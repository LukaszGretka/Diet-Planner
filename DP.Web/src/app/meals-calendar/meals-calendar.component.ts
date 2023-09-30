import { Component, OnInit } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { BehaviorSubject, exhaustMap, map, Observable, of, take } from 'rxjs';
import { ProductService } from '../products/services/product.service';
import { DatePickerSelection } from './models/date-picker-selection';
import { MealType } from './models/meal-type';
import { MealCalendarState } from './stores/meals-calendar.state';
import * as MealCalendarActions from './stores/meals-calendar.actions';
import * as MealCalendarSelectors from './stores/meals-calendar.selectors';
import * as GeneralSelector from './../stores/store.selectors';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChartData, ChartType } from 'chart.js';
import { Actions, ofType } from '@ngrx/effects';
import * as ProductsSelectors from './../products/stores/products.selectors';
import { Meal } from './models/meal';
import { ProductsState } from '../products/stores/products.state';
import * as ProductActions from '../products/stores/products.actions';
import * as GeneralActions from '../stores/store.actions';
import { DishProduct } from '../dishes/models/dish-product';
import { Dish } from '../dishes/models/dish';

@UntilDestroy()
@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css'],
})
export class MealsCalendarComponent implements OnInit {
  //TODO move to effect
  //may require refactor if list of products will be long (need to test it)
  public productsNames$: Observable<string[]> = this.productService
    .getProductsWithPortion()
    .pipe(map(products => products.map(product => product.name)));

  public currentProducts: string[];
  public totalCalories: number;

  public dailyMealsOverview$ = this.store.select(MealCalendarSelectors.getDailyMealsOverview);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  public breakfastDishes$ = new BehaviorSubject<Dish[]>([]);
  public lunchDishes$ = new BehaviorSubject<Dish[]>([]);
  public dinnerDishes$ = new BehaviorSubject<Dish[]>([]);
  public supperDishes$ = new BehaviorSubject<Dish[]>([]);

  public dateModel: DatePickerSelection;
  public breakfastSearchModel: string;
  public lunchSearchModel: string;
  public dinnerSearchModel: string;

  public isBreakfastVisible = true;
  public isLunchVisible = true;
  public isDinnerVisible = true;
  public isSupperVisible = true;
  public selectedDate: Date;

  public doughnutChartLabels: string[] = ['Carbohydrates', 'Proteins', 'Fats'];
  public doughnutChartData: ChartData<'doughnut'> = {
    labels: this.doughnutChartLabels,
    datasets: [{ data: [0, 0, 0] }],
  };
  public doughnutChartType: ChartType = 'doughnut';

  private callbackMealProduct$ = this.store.select(ProductsSelectors.getCallbackMealProduct);

  constructor(
    actions$: Actions,
    private productService: ProductService,
    private store: Store<MealCalendarState>,
    private productStore: Store<ProductsState>,
  ) {
    actions$.pipe(ofType(MealCalendarActions.addMealRequestSuccess), untilDestroyed(this)).subscribe(() => {
      this.totalCalories = this.calculateTotalCalories();
      this.doughnutChartData = {
        labels: this.doughnutChartLabels,
        datasets: [{ data: this.buildMacronutrientsChartDataset() }],
      };
    });
  }

  ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear(),
    };

    this.selectedDate = dateNow;
    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: this.selectedDate }));

    this.dailyMealsOverview$.pipe(untilDestroyed(this)).subscribe(meals => {
      this.breakfastDishes$.next(meals.filter(m => m.mealTypeId === MealType.breakfast)[0]?.dishes ?? []);
      this.lunchDishes$.next(meals.filter(m => m.mealTypeId === MealType.lunch)[0]?.dishes ?? []);
      this.dinnerDishes$.next(meals.filter(m => m.mealTypeId === MealType.dinner)[0]?.dishes ?? []);
      this.supperDishes$.next(meals.filter(m => m.mealTypeId === MealType.supper)[0]?.dishes ?? []);
      this.totalCalories = this.calculateTotalCalories();
      this.doughnutChartData = {
        labels: this.doughnutChartLabels,
        datasets: [{ data: this.buildMacronutrientsChartDataset() }],
      };
    });

    this.productsNames$.pipe(untilDestroyed(this)).subscribe(productNames => {
      this.currentProducts = productNames;
    });

    this.callbackMealProduct$.pipe(take(1)).subscribe(callback => {
      if (!callback) {
        return;
      }
      const targetedMeal = this.dailyMealsOverview$.pipe(untilDestroyed(this)).pipe(
        exhaustMap(meals => {
          var targetedMeal = meals.filter(meal => meal.mealTypeId == callback.mealType);
          return of(targetedMeal[0]);
        }),
      );
      const targetedMealSb = new BehaviorSubject<Meal>(null);
      targetedMeal.subscribe(targetedMealSb);

      let currentDishes = [...targetedMealSb.getValue().dishes];

      this.productService
        .getProductByName(callback.productName)
        .pipe(take(1))
        .subscribe(product => {
          // currentDishes.push({ product, portionMultiplier: 1 });
          // targetedMealSb.next({ mealTypeId: callback.mealType, portionProducts: currentDishes });
          // this.store.dispatch(
          //   MealCalendarActions.addMealRequest({
          //     mealByDay: {
          //       date: this.selectedDate,
          //       portionProducts: targetedMealSb.getValue().portionProducts,
          //       mealTypeId: callback.mealType,
          //     },
          //   }),
          // );
        });
    });
    this.productStore.dispatch(ProductActions.clearCallbackMealProduct());
  }

  public onDateSelection(ngbDate: NgbDate): void {
    const selectedDate = new Date();
    selectedDate.setFullYear(ngbDate.year);
    selectedDate.setMonth(ngbDate.month - 1);
    selectedDate.setDate(ngbDate.day);
    this.selectedDate = selectedDate;
    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: this.selectedDate }));
  }

  private buildMacronutrientsChartDataset(): [number, number, number] {
    const breakfastMacros = this.calculateTotalMacronutrientsForMealType(this.breakfastDishes$);
    const lunchMacros = this.calculateTotalMacronutrientsForMealType(this.lunchDishes$);
    const dinnerMacros = this.calculateTotalMacronutrientsForMealType(this.dinnerDishes$);
    const supperProducts = this.calculateTotalMacronutrientsForMealType(this.supperDishes$);

    return [
      breakfastMacros.carbs + lunchMacros.carbs + dinnerMacros.carbs + supperProducts.carbs,
      breakfastMacros.proteins + lunchMacros.proteins + dinnerMacros.proteins + supperProducts.proteins,
      breakfastMacros.fats + lunchMacros.fats + dinnerMacros.fats + supperProducts.fats,
    ];
  }

  private calculateTotalCalories(): number {
    let totalSum = 0;
    this.breakfastDishes$.getValue().forEach((dish: Dish) => {
      dish.products.forEach(dishProduct => {
        totalSum += dishProduct.product.calories;
      });
    });

    this.lunchDishes$.getValue().forEach((dish: Dish) => {
      dish.products.forEach(dishProduct => {
        totalSum += dishProduct.product.calories;
      });
    });

    this.dinnerDishes$.getValue().forEach((dish: Dish) => {
      dish.products.forEach(dishProduct => {
        totalSum += dishProduct.product.calories;
      });
    });

    this.supperDishes$.getValue().forEach((dish: Dish) => {
      dish.products.forEach(dishProduct => {
        totalSum += dishProduct.product.calories;
      });
    });

    return totalSum;
  }

  private calculateTotalMacronutrientsForMealType(mealTypeProducts$: any) {
    let macronutrients = { carbs: 0, proteins: 0, fats: 0 };

    mealTypeProducts$.getValue().forEach((dishProduct: DishProduct) => {
      (macronutrients.carbs += dishProduct.product?.carbohydrates),
        (macronutrients.proteins += dishProduct.product?.proteins),
        (macronutrients.fats += dishProduct.product?.fats);
    });

    return macronutrients;
  }
}
