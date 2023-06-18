import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import * as MealCalendarSelectors from './../stores/meals-calendar.selectors';
import { Product } from 'src/app/products/models/product';
import { ProductService } from 'src/app/products/services/product.service';
import { MealCalendarState } from '../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../models/meal-type';
import { Meal } from '../models/meal';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-template',
  templateUrl: './meal-calendar-template.component.html',
  styleUrls: ['./meal-calendar-template.component.css']
})
export class MealCalendarTemplateComponent implements OnInit {

  @Input()
  public mealProducts$: BehaviorSubject<(Meal[])>;

  @Input()
  public selectedDate: Date;

  //TODO move to effect
  //may require refactor if list of products will be long (need to test it)
  public productsNames$: Observable<string[]> = this.productService
    .getProducts()
    .pipe(map(products => products.map(product => product.name)));

  public searchItem: string;
  public currentProducts: string[];

  constructor(private productService: ProductService, private store: Store<MealCalendarState>) { }

  ngOnInit(): void {
    this.productsNames$.pipe(
      untilDestroyed(this))
      .subscribe(productNames => {
        this.currentProducts = productNames;
      });
  }

  // Update local products list for particular collection given in parameter.
  public onAddProductButtonClick(behaviorSubject: BehaviorSubject<any>, productName: string): void {
    if (productName) {
      this.productService
        .getProductByName(productName)
        .pipe(untilDestroyed(this))
        .subscribe(product => {
          if (product) {
            const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
            const products = (productsBehaviorSubject.getValue() as Product[]).concat(product);
            productsBehaviorSubject.next(products);
          } else {
            //TODO: product no exists, want to add a product with this name?
          }
        });
    }
  }

  // Remove product from local list by given index
  public onRemoveProductButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
    const products = productsBehaviorSubject.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    productsBehaviorSubject.next(productsLocal);
  }

  public onMealSaveButtonClick(behaviorSubject: BehaviorSubject<any>, mealType: MealType) {
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          products: behaviorSubject.getValue(),
          mealTypeId: mealType,
        },
      }),
    );
  }

  searchProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(searchText =>
        searchText.length < 1
          ? []
          : this.currentProducts
            .filter(product => product.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
            .slice(0, 10),
      ),
    );
}
