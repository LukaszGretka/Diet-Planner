import { Injectable } from '@angular/core';
import { MealsCalendarService } from '../services/meals-calendar.service';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, mergeMap, of, switchMap, tap } from 'rxjs';
import * as MealCalendarActions from './meals-calendar.actions';
import * as GeneralActions from './../../stores/store.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Injectable()
export class MealCalendarEffects {
  getMealsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.getMealsRequest),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.getDailyMeals(payload.date).pipe(
          switchMap(result => of(MealCalendarActions.getMealsRequestSuccess({ result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  addMealEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.addMealRequest),
      mergeMap(({ payload }) => {
        return this.mealsCalendarService.addDialyMeal(payload.mealByDay).pipe(
          switchMap(() => of(MealCalendarActions.addMealRequestSuccess({ addedDate: payload.mealByDay.date }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  addMealSuccessEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.addMealRequestSuccess),
      tap(() => this.notificationService.showSuccessToast('Changes saved', 'Meals have been successfully updated.')),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.getDailyMeals(payload.addedDate).pipe(
          switchMap(result => of(MealCalendarActions.getMealsRequestSuccess({ result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  constructor(
    private actions$: Actions,
    private mealsCalendarService: MealsCalendarService,
    private notificationService: NotificationService,
  ) {}
}
