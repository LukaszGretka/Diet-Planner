import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import { PortionProduct, Product } from 'src/app/products/models/product';
import { ProductService } from 'src/app/products/services/product.service';
import { MealCalendarState } from '../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../models/meal-type';
import { Meal } from '../models/meal';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as MealCalendarSelectors from './../stores/meals-calendar.selectors';
import * as ProductsActions from '../../products/stores/products.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as ProductSelectors from './../../products/stores/products.selectors';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-template',
  templateUrl: './meal-calendar-template.component.html',
  styleUrls: ['./meal-calendar-template.component.css'],
})
export class MealCalendarTemplateComponent implements OnInit {
  @Input()
  public portionProducts$: BehaviorSubject<PortionProduct[]>;

  @Input()
  public selectedDate: Date;

  @Input()
  public mealType: MealType;

  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.store.select(ProductSelectors.getCallbackMealProduct);

  public searchItem: string;
  public currentProducts: PortionProduct[];
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  public mealMacroSummary$: Observable<any>;

  constructor(
    private productService: ProductService,
    private store: Store<MealCalendarState>,
    private router: Router,
    private modalService: NgbModal,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.store.dispatch(ProductsActions.getAllProductsRequest());
    this.mealMacroSummary$ = this.portionProducts$.pipe(
      map(product =>
        product.reduce(
          (total, product) => {
            (total.calories += product.calories),
              (total.carbohydrates += product.carbohydrates),
              (total.proteins += product.proteins),
              (total.fats += product.fats);
            return total;
          },
          {
            calories: 0,
            carbohydrates: 0,
            proteins: 0,
            fats: 0,
          },
        ),
      ),
    );
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
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<PortionProduct[]>;
    const products = productsBehaviorSubject.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    productsBehaviorSubject.next(productsLocal);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          portionProducts: productsBehaviorSubject.getValue(),
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

  private addFoundProduct(behaviorSubject: BehaviorSubject<any>, foundProduct: PortionProduct): boolean {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<PortionProduct[]>;
    if (productsBehaviorSubject.getValue().filter(p => p.id == foundProduct.id).length > 0) {
      this.notificationService.showWarningToast(
        'Product already exist in this meal.',
        'Please edit portion box to adjust the entry.',
        5000,
      );
      return;
    }
    const products = (productsBehaviorSubject.getValue() as PortionProduct[]).concat(foundProduct);
    productsBehaviorSubject.next(products);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          portionProducts: productsBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public onPortionValueChange(poritonSize: number, behaviorSubject: BehaviorSubject<any>, index: number) {
    this.store.dispatch(
      MealCalendarActions.updatePortionRequest({
        date: this.selectedDate,
        mealType: this.mealType,
        productId: behaviorSubject.getValue()[index].id,
        portionMultiplier: poritonSize / this.defaultPortionSize,
      }),
    );
  }

  public getTotalMealCalories(): Observable<number> {
    return new BehaviorSubject(1);
  }
}
