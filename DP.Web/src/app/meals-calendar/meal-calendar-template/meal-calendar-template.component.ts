import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import { Product } from 'src/app/products/models/product';
import { ProductService } from 'src/app/products/services/product.service';
import { MealCalendarState } from '../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../models/meal-type';
import { Meal } from '../models/meal';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as MealCalendarSelectors from './../stores/meals-calendar.selectors';
import * as ProductsActions from '../../products/stores/products.actions';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-template',
  templateUrl: './meal-calendar-template.component.html',
  styleUrls: ['./meal-calendar-template.component.css'],
})
export class MealCalendarTemplateComponent implements OnInit {
  @Input()
  public mealProducts$: BehaviorSubject<Meal[]>;

  @Input()
  public selectedDate: Date;

  @Input()
  public mealType: MealType;


  //TODO move to effect
  //may require refactor if list of products will be long (need to test it)
  public products$: Observable<Product[]> = this.productService
    .getProducts()
    .pipe(map(products => products.map(product => product)));

  public searchItem: string;
  public currentProducts: Product[];
  public dailyMealsOverview$ = this.store.select(MealCalendarSelectors.getDailyMealsOverview);
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  constructor(
    private productService: ProductService,
    private store: Store<MealCalendarState>,
    private router: Router,
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.products$.pipe(untilDestroyed(this)).subscribe(products => {
      this.currentProducts = products;
    });
  }

  // Update local products list for particular collection given in parameter.
  public onAddProductButtonClick(behaviorSubject: BehaviorSubject<any>, productName: string, content: any): void {
    if (productName) {
      const foundProduct = this.currentProducts.find(p => p.name === productName);
      if (foundProduct) {
        this.addFoundProduct(behaviorSubject, foundProduct);
        this.searchItem = '';
      } else {
        this.modalService.open(content);
      }
    }
  }

  public onCancelModalClick() {
    this.modalService.dismissAll();
  }

  // Remove product from local list by given index
  public onRemoveProductButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
    const products = productsBehaviorSubject.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    productsBehaviorSubject.next(productsLocal);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          products: productsBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public addNewProductModalButtonClick(): void {
    this.modalService.dismissAll();
    this.store.dispatch(
      ProductsActions.setCallbackMealProduct({ productName: this.searchItem, mealType: this.mealType }),
    );
    this.router.navigateByUrl(`products/add?redirectUrl=${this.router.url}`);
  }

  public searchProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(searchText =>
        searchText.length < 1
          ? []
          : this.currentProducts
            .filter(product => product.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
            .slice(0, 10)
            .map(p => p.name),
      ),
    );

  private addFoundProduct(behaviorSubject: BehaviorSubject<any>, foundProduct: Product) {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
    const products = (productsBehaviorSubject.getValue() as Product[]).concat(foundProduct);
    productsBehaviorSubject.next(products);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          products: productsBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public onPortionValueChange(value) {
    if (value == this.defaultPortionSize) {
      //   return 1;
      // }
    }
  }
