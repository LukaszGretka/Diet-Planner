import {Injectable} from '@angular/core';
import {MealsCalendarService} from '../services/meals-calendar.service';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {catchError, exhaustMap, of, switchMap} from 'rxjs';
import * as MealCalendarActions from './meals-calendar.actions';
import * as GeneralActions from './../../stores/store.actions';

@Injectable()
export class MealCalendarEffects {
  getMealsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.getMealsRequest),
      exhaustMap(({payload}) => {
        return this.mealsCalendarService.getDailyMeals(payload.date).pipe(
          switchMap(result => of(MealCalendarActions.getMealsRequestSuccess({result}))),
          catchError((error: any) => of(GeneralActions.setErrorCode({errorCode: error.status}))),
        );
      }),
    ),
  );

  addMealEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.addMealRequest),
      switchMap(({payload}) => {
        return this.mealsCalendarService.addDialyMeal(payload.mealByDay).pipe(
          switchMap(() => of(MealCalendarActions.addMealRequestSuccess())),
          catchError((error: number) => of(GeneralActions.setErrorCode({errorCode: error}))),
        );
      }),
    ),
  );

  constructor(private actions$: Actions, private mealsCalendarService: MealsCalendarService) {}
}
