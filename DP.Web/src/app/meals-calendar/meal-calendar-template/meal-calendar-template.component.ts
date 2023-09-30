import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import * as DishActions from './../../dishes/stores/dish.actions';
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
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { DishState } from 'src/app/dishes/stores/dish.state';
import * as DishSelectors from './../../dishes/stores/dish.selectors';
import { Dish } from 'src/app/dishes/models/dish';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-template',
  templateUrl: './meal-calendar-template.component.html',
  styleUrls: ['./meal-calendar-template.component.css'],
})
export class MealCalendarTemplateComponent implements OnInit {
  @Input()
  public dishes$: BehaviorSubject<Dish[]>;

  @Input()
  public selectedDate: Date;

  @Input()
  public mealType: MealType;

  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.store.select(ProductSelectors.getCallbackMealProduct);
  public allDishes$ = this.dishStore.select(DishSelectors.getDishes);

  public searchItem: string;
  public currentProducts: DishProduct[];
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  public mealMacroSummary$: Observable<any>;

  constructor(
    private store: Store<MealCalendarState>,
    private dishStore: Store<DishState>,
    private router: Router,
    private modalService: NgbModal,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.store.dispatch(ProductsActions.getAllProductsRequest());
    this.mealMacroSummary$ = this.dishes$.pipe(
      map(dishes =>
        dishes.reduce(
          (total, dish: Dish) => {
            dish.products.forEach((dishProduct: DishProduct) => {
              (total.calories += dishProduct.product.calories),
                (total.carbohydrates += dishProduct.product.carbohydrates),
                (total.proteins += dishProduct.product.proteins),
                (total.fats += dishProduct.product.fats);
            });
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
      const foundProduct = this.currentProducts.find(dishProduct => dishProduct.product.name === productName);
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
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<DishProduct[]>;
    const products = productsBehaviorSubject.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    productsBehaviorSubject.next(productsLocal);
    // this.store.dispatch(
    //   MealCalendarActions.addMealRequest({
    //     mealByDay: {
    //       date: this.selectedDate,
    //       portionProducts: productsBehaviorSubject.getValue(),
    //       mealTypeId: this.mealType,
    //     },
    //   }),
    // );
  }

  public onShowProductButtonClick($event: any) {
    let buttonId = $event.delegateTarget.id as string;
    var hideButtonId = buttonId.replace('show', 'hide');
    document.getElementById(buttonId).style.display = 'none';
    document.getElementById(hideButtonId).style.display = 'inline-block';
  }

  public onHideProductButtonClick($event: any) {
    let buttonId = $event.delegateTarget.id as string;
    var showButtonId = buttonId.replace('hide', 'show');
    document.getElementById(buttonId).style.display = 'none';
    document.getElementById(showButtonId).style.display = 'inline-block';
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
              .filter(dishProduct => dishProduct.product.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
              .slice(0, 10)
              .map(dishProduct => dishProduct.product.name),
      ),
    );

  private addFoundProduct(behaviorSubject: BehaviorSubject<any>, foundProduct: DishProduct): boolean {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<DishProduct[]>;
    if (
      productsBehaviorSubject.getValue().filter(dishProduct => dishProduct.product.id == foundProduct.product.id)
        .length > 0
    ) {
      this.notificationService.showWarningToast(
        'Product already exist in this meal.',
        'Please edit portion box to adjust the entry.',
        5000,
      );
      return;
    }
    const products = (productsBehaviorSubject.getValue() as DishProduct[]).concat(foundProduct);
    productsBehaviorSubject.next(products);
    // this.store.dispatch(
    //   MealCalendarActions.addMealRequest({
    //     mealByDay: {
    //       date: this.selectedDate,
    //       portionProducts: productsBehaviorSubject.getValue(),
    //       mealTypeId: this.mealType,
    //     },
    //   }),
    // );
  }

  public onPortionValueChange(
    dishId: number,
    poritonSize: number,
    behaviorSubject: BehaviorSubject<any>,
    index: number,
  ) {
    this.store.dispatch(
      DishActions.updatePortionRequest({
        dishId: dishId,
        productId: behaviorSubject.getValue()[index].id,
        portionMultiplier: poritonSize / this.defaultPortionSize,
      }),
    );
  }

  public calculateDishMacros(dish: Dish): { carbs: number; proteins: number; fats: number; calories: number } {
    let dishMacro = { carbs: 0, proteins: 0, fats: 0, calories: 0 };
    dish.products.forEach(p => {
      dishMacro.carbs += p.product.carbohydrates;
      dishMacro.proteins += p.product.proteins;
      dishMacro.fats += p.product.fats;
      dishMacro.calories += p.product.calories;
    });

    console.log(dishMacro);
    return dishMacro;
  }
}
