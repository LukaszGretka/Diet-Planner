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

  updatePortionEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.updatePortionRequest),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService
          .updatePortionMultiplier(payload.date, payload.mealType, payload.productId, payload.portionMultiplier)
          .pipe(
            switchMap(() => of(MealCalendarActions.updatePortionRequestSuccess())),
            catchError(() => of(MealCalendarActions.updatePortionRequestFailed())),
          );
      }),
    ),
  );

  updatePortionEffectSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.updatePortionRequestSuccess),
        tap(() =>
          this.notificationService.showSuccessToast('Changes saved', 'Portion have been successfully updated.'),
        ),
      ),
    {
      dispatch: false,
    },
  );

  updatePortionEffectFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.updatePortionRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during saving portion.')),
      ),
    {
      dispatch: false,
    },
  );

  constructor(
    private actions$: Actions,
    private mealsCalendarService: MealsCalendarService,
    private notificationService: NotificationService,
  ) {}
}
