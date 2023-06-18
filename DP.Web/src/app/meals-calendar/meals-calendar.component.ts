import { Component, OnInit } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import { BehaviorSubject, debounceTime, distinctUntilChanged, map, Observable, OperatorFunction } from 'rxjs';
import { Product } from '../products/models/product';
import { ProductService } from '../products/services/product.service';
import { DatePickerSelection } from './models/date-picker-selection';
import { MealType } from './models/meal-type';
import { MealCalendarState } from './stores/meals-calendar.state';
import * as MealCalendarActions from './stores/meals-calendar.actions';
import * as MealCalendarSelectors from './stores/meals-calendar.selectors';
import * as GeneralSelector from './../stores/store.selectors';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { ChartData, ChartType } from 'chart.js';
import { Meal } from './models/meal';

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
    .getProducts()
    .pipe(map(products => products.map(product => product.name)));

  public currentProducts: string[];

  public dailyMealsOverview$ = this.store.select(MealCalendarSelectors.getDailyMealsOverview);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  public breakfastProducts$ = new BehaviorSubject([]);
  public lunchProducts$ = new BehaviorSubject([]);
  public dinnerProducts$ = new BehaviorSubject([]);
  public supperProducts$ = new BehaviorSubject([]);

  public dateModel: DatePickerSelection;
  public breakfastSearchModel: string;
  public lunchSearchModel: string;
  public dinnerSearchModel: string;

  public selectedDate: Date;

  public chartData: ChartData<'doughnut', number[], string | string[]> = {
    labels: ['Carbohydrates', 'Proteins', 'Fats'],
    datasets: [{ data: [0, 0, 0] }],
  };

  constructor(private productService: ProductService, private store: Store<MealCalendarState>) {}

  ngOnInit(): void {
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear(),
    };

    this.selectedDate = dateNow;
    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: this.selectedDate }));

    this.dailyMealsOverview$.pipe(untilDestroyed(this)).subscribe(meals => {
      this.breakfastProducts$.next(meals.filter(m => m.mealTypeId === MealType.breakfast)[0]?.products ?? []);
      this.lunchProducts$.next(meals.filter(m => m.mealTypeId === MealType.lunch)[0]?.products ?? []);
      this.dinnerProducts$.next(meals.filter(m => m.mealTypeId === MealType.dinner)[0]?.products ?? []);
      this.supperProducts$.next(meals.filter(m => m.mealTypeId === MealType.supper)[0]?.products ?? []);
      this.chartData.datasets = [{ data: this.buildMacronutrientsChartDataset() }];
    });

    this.productsNames$.pipe(untilDestroyed(this)).subscribe(productNames => {
      this.currentProducts = productNames;
    });
  }

  public onDateSelection(ngbDate: NgbDate): void {
    const selectedDate = new Date();
    selectedDate.setFullYear(ngbDate.year);
    selectedDate.setMonth(ngbDate.month - 1);
    selectedDate.setDate(ngbDate.day);
    this.selectedDate = selectedDate;
    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: this.selectedDate }));
  }

  //TODO: MUST be redesigned after tests
 
  public calculateTotalCalories(): number {
    let totalSum = 0;
    this.breakfastProducts$.getValue().forEach((product: Product) => {
      totalSum += product.calories;
    });
    this.lunchProducts$.getValue().forEach((product: Product) => {
      totalSum += product.calories;
    });
    this.dinnerProducts$.getValue().forEach((product: Product) => {
      totalSum += product.calories;
    });

    return totalSum;
  }

  private buildMacronutrientsChartDataset(): [number, number, number] {
    const breakfastMacros = this.calculateTotalMacronutrientsForMealType(this.breakfastProducts$);
    const lunchMacros = this.calculateTotalMacronutrientsForMealType(this.lunchProducts$);
    const dinnerMacros = this.calculateTotalMacronutrientsForMealType(this.dinnerProducts$);

    return [
      breakfastMacros.carbs + lunchMacros.carbs + dinnerMacros.carbs,
      breakfastMacros.proteins + lunchMacros.proteins + dinnerMacros.proteins,
      breakfastMacros.fats + lunchMacros.fats + dinnerMacros.fats,
    ];
  }

  private calculateTotalMacronutrientsForMealType(mealTypeProducts$: any) {
    let macronutrients = { carbs: 0, proteins: 0, fats: 0 };

    mealTypeProducts$.getValue().forEach((product: Product) => {
      (macronutrients.carbs += product.carbohydrates),
        (macronutrients.proteins += product.proteins),
        (macronutrients.fats += product.fats);
    });

    return macronutrients;
  }
}
