import { Injectable, inject } from '@angular/core';
import { MealsCalendarService } from '../services/meals-calendar.service';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, mergeMap, of, switchMap, tap } from 'rxjs';
import * as MealCalendarActions from './meals-calendar.actions';
import * as GeneralActions from './../../stores/store.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';

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
          switchMap(() => of(MealCalendarActions.addMealRequestSuccess({ addedDate: payload.addMealRequest.date }))),
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
        return this.mealsCalendarService.getAllMeals(payload.addedDate).pipe(
          switchMap(result => of(MealCalendarActions.getAllMealsRequestSuccess({ result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  removeMealEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.removeMealItemRequest),
      mergeMap(({ payload }) => {
        return this.mealsCalendarService.removeItemFromMeal(payload.removeMealRequest).pipe(
          switchMap(() => of(MealCalendarActions.removeMealItemSuccess({ addedDate: payload.removeMealRequest.date }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  removeMealSuccessEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.removeMealItemSuccess),
      tap(() => this.notificationService.showSuccessToast('Changes saved', 'Meal item has been successfully removed.')),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.getAllMeals(payload.addedDate).pipe(
          switchMap(result => of(MealCalendarActions.getAllMealsRequestSuccess({ result }))),
          catchError((error: any) => of(GeneralActions.setErrorCode({ errorCode: error.status }))),
        );
      }),
    ),
  );

  updatePortionRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(MealCalendarActions.updateMealItemPortionRequest),
      exhaustMap(({ payload }) => {
        return this.mealsCalendarService.updateMealItemPortion(payload.request).pipe(
          switchMap(() =>
            of(
              MealCalendarActions.updateMealItemPortionSuccess(),
              MealCalendarActions.getAllMealsRequest({ date: payload.request.date }),
            ),
          ),
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
