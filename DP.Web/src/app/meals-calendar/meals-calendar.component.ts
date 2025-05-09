import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject } from 'rxjs';
import { MealType } from './models/meal-type';
import { MealCalendarState } from './stores/meals-calendar.state';
import * as MealCalendarActions from './stores/meals-calendar.actions';
import * as MealCalendarSelectors from './stores/meals-calendar.selectors';
import * as GeneralSelector from './../stores/store.selectors';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import * as GeneralActions from '../stores/store.actions';
import { MealConfig } from './models/meal-calendar-config';
import { DailyMeals } from './models/daily-meals';
import { Meal } from './models/meal';
import { AsyncPipe } from '@angular/common';
import { ErrorPageComponent } from '../shared/error-page/error-page.component';
import { DateSelectionComponent } from './components/date-selection/date-selection.component';
import { MealCalendarChartComponent } from './components/meal-calendar-chart/meal-calendar-chart.component';
import { MealSectionComponent } from './components/meal-section/meal-section.component';

@UntilDestroy()
@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css'],
  imports: [ErrorPageComponent, DateSelectionComponent, MealCalendarChartComponent, MealSectionComponent, AsyncPipe],
})
export class MealsCalendarComponent implements OnInit {
  private readonly store = inject<Store<MealCalendarState>>(Store);

  public mealCalendarConfig: MealConfig[] = [
    { name: 'Breakfast', type: MealType.breakfast, isVisible: true },
    { name: 'Lunch', type: MealType.lunch, isVisible: true },
    { name: 'Dinner', type: MealType.dinner, isVisible: true },
    { name: 'Supper', type: MealType.supper, isVisible: true },
  ];

  public dailyMeals$: BehaviorSubject<DailyMeals> = new BehaviorSubject<DailyMeals>(null);
  public allDailyMeals$ = this.store.select(MealCalendarSelectors.getAllDailyMeals);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);
  public selectedDate: Date;

  public ngOnInit(): void {
    this.allDailyMeals$.pipe(untilDestroyed(this)).subscribe(meals => {
      this.dailyMeals$.next({
        meals: {
          Breakfast:
            meals.filter(meal => meal.mealType === MealType.breakfast)[0] ?? this.getDefaultMeal(MealType.breakfast),
          Lunch: meals.filter(meal => meal.mealType === MealType.lunch)[0] ?? this.getDefaultMeal(MealType.lunch),
          Dinner: meals.filter(meal => meal.mealType === MealType.dinner)[0] ?? this.getDefaultMeal(MealType.dinner),
          Supper: meals.filter(meal => meal.mealType === MealType.supper)[0] ?? this.getDefaultMeal(MealType.supper),
        },
      });
    });

    this.store.dispatch(GeneralActions.clearErrors());
  }

  public setDate(date: Date) {
    this.selectedDate = date;
    this.store.dispatch(MealCalendarActions.getAllMealsRequest({ date: date }));
  }

  private getDefaultMeal(mealType: MealType): Meal {
    return {
      date: this.selectedDate,
      mealType: mealType,
      dishes: [],
      products: [],
    };
  }
}
