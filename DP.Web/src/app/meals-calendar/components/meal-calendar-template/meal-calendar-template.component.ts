import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import {
  BehaviorSubject,
  Observable,
  OperatorFunction,
  debounceTime,
  distinctUntilChanged,
  map,
  of,
  switchMap,
  take,
  combineLatest,
} from 'rxjs';
import * as MealCalendarActions from '../../stores/meals-calendar.actions';
import { MealCalendarState } from '../../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../../models/meal-type';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { DishState } from 'src/app/dishes/stores/dish.state';
import { Dish } from 'src/app/dishes/models/dish';
import * as ProductsActions from '../../../products/stores/products.actions';
import * as DishActions from '../../../dishes/stores/dish.actions';
import {ProductsState} from "../../../products/stores/products.state";
import { BaseItem, ItemType } from 'src/app/shared/models/base-item';

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

  public searchItem: string;
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  public mealMacroSummary$: Observable<any>;

  constructor(
    private store: Store<MealCalendarState>,
    private dishStore: Store<DishState>,
    private productStore: Store<ProductsState>,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.dishStore.dispatch(DishActions.loadDishesRequest());
    this.productStore.dispatch(ProductsActions.getAllProductsRequest());
    this.mealMacroSummary$ = this.dishes$.pipe(
      map(dishes =>
        dishes.reduce(
          (total, dish: Dish) => {
            dish.products.forEach((dishProduct: DishProduct) => {
              (total.calories += dishProduct.product.calories * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier)),
                (total.carbohydrates += dishProduct.product.carbohydrates * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier)),
                (total.proteins += dishProduct.product.proteins * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier)),
                (total.fats += dishProduct.product.fats * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier));
            });
            return total;
          },
          { calories: 0, carbohydrates: 0, proteins: 0, fats: 0 },
        ),
      ),
    );
  }

  // Remove dish from local list by given index
  public onRemoveDishButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const dishesBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dishes = dishesBehaviorSubject.getValue();
    let localDishes = [...dishes];
    localDishes.splice(index, 1);
    dishesBehaviorSubject.next(localDishes);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          dishes: dishesBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public async onEditDishButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): Promise<void> {
    const dishesBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dish: Dish = dishesBehaviorSubject.getValue()[index];
    await this.router.navigateByUrl(`dishes/edit/${dish.id}?redirectUrl=${this.router.url}`);
  }

  private addFoundDish(behaviorSubject: BehaviorSubject<any>, foundDishes: Dish[]): void {
    const dishBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dishes = (dishBehaviorSubject.getValue() as Dish[]).concat(foundDishes);
    dishBehaviorSubject.next(dishes);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          dishes: dishBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public onPortionValueChange(customizedPortionSize: number, dishId: number, mealDishId: number, productId: number) {
    this.store.dispatch(
      DishActions.updatePortionRequest({
        dishId: dishId,
        productId: productId,
        mealDishId: mealDishId,
        customizedPortionMultiplier: customizedPortionSize / this.defaultPortionSize,
        date: this.selectedDate,
      }),
    );
  }

  public calculateDishMacros(dish: Dish): { carbs: number; proteins: number; fats: number; calories: number } {
    let dishMacro = { carbs: 0, proteins: 0, fats: 0, calories: 0 };
    dish.products.forEach(dishProduct => {
      dishMacro.carbs += dishProduct.product.carbohydrates * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.proteins += dishProduct.product.proteins * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.fats += dishProduct.product.fats * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.calories += dishProduct.product.calories * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
    });

    return dishMacro;
  }

  public itemAddedToSearchBar(item: BaseItem){
    console.log('added: ' + item.name);


    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          dishes: dishBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );

    // if(searchItem.itemType === ItemType.Product){
    //   console.log('should add product')
    //   //TODO: Emit product to be added in meal-calendar-component
    // }
    //
    // if(searchItem.itemType === ItemType.Dish) {
    //   console.log('should add dish')
    //   //TODO: Emit dish to be added in meal-calendar-component
    // }
  }
}
