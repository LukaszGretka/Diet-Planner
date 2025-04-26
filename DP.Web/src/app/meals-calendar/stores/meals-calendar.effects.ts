import { Injectable, inject } from '@angular/core';
import { MealsCalendarService } from '../services/meals-calendar.service';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, mergeMap, of, switchMap, tap } from 'rxjs';
import * as MealCalendarActions from './meals-calendar.actions';
import * as GeneralActions from './../../stores/store.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { Meal } from '../models/meal';

@Injectable()
export class MealCalendarEffects {
  private readonly actions$ = inject(Actions);
  private readonly mealsCalendarService = inject(MealsCalendarService);
  private readonly notificationService = inject(NotificationService);

  getMealsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.getAllMealsRequest),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.getAllMeals(payload.date).pipe(
          switchMap(result => of(MealCalendarActions.getAllMealsRequestSuccess({ result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  addMealEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.addMealRequest),
      mergeMap(({ payload }) => {
        return this.mealsCalendarService.addItemToMeal(payload.addMealRequest).pipe(
          switchMap((result: Meal[]) => of(MealCalendarActions.addMealRequestSuccess({ result: result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  addMealSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.addMealRequestSuccess),
        tap(() => this.notificationService.showSuccessToast('Changes saved', 'Meals have been successfully updated.')),
      ),
    { dispatch: false },
  );

  removeMealEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.removeMealItemRequest),
      mergeMap(({ payload }) => {
        return this.mealsCalendarService.removeItemFromMeal(payload.removeMealRequest).pipe(
          switchMap((result: Meal[]) => of(MealCalendarActions.removeMealItemSuccess({ result: result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  removeMealSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.removeMealItemSuccess),
        tap(() =>
          this.notificationService.showSuccessToast('Changes saved', 'Meal item has been successfully removed.'),
        ),
      ),
    { dispatch: false },
  );

  updatePortionRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.updateMealItemPortionRequest),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.updateMealItemPortion(payload.request).pipe(
          switchMap((result: Meal[]) => of(MealCalendarActions.updateMealItemPortionSuccess({ result: result }))),
          catchError(error => of(MealCalendarActions.updateMealItemPortionFailed(error))),
        );
      }),
    ),
  );
  updatePortionEffectSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.updateMealItemPortionSuccess),
        tap(() => {
          return this.notificationService.showSuccessToast('Changes saved', 'Portion have been successfully updated.');
        }),
      ),
    {
      dispatch: false,
    },
  );

  updatePortionEffectFailed$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(MealCalendarActions.updateMealItemPortionFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during saving portion.')),
      ),
    {
      dispatch: false,
    },
  );
}
