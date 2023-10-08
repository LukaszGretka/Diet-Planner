import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DishService } from '../services/dish.service';
import { catchError, exhaustMap, of, switchMap, tap } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as DishActions from './dish.actions';
import * as MealCalendarActions from './../../meals-calendar/stores/meals-calendar.actions';
import { Router } from '@angular/router';

@Injectable()
export class DishEffects {
  constructor(
    private actions$: Actions,
    private dishService: DishService,
    private notificationService: NotificationService,
    private router: Router,
  ) {}

  loadDishesEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.loadDishesRequest),
      exhaustMap(() => {
        return this.dishService.getUserDishes().pipe(
          switchMap(dishes => of(DishActions.loadDishesRequestSuccess({ dishes }))),
          catchError(error => of(DishActions.loadDishesRequestFailed(error))),
        );
      }),
    ),
  );

  loadDishesFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.loadDishesRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during loading dishes.')),
      ),
    {
      dispatch: false,
    },
  );

  saveDishEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.saveDishRequest),
      exhaustMap(({ payload }) => {
        return this.dishService.saveDish(payload.dish).pipe(
          switchMap(() => of(DishActions.saveDishRequestSuccess())),
          catchError(error => of(DishActions.saveDishRequestFailed(error))),
        );
      }),
    ),
  );

  saveDishSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.saveDishRequestSuccess),
        tap(() => {
          this.router.navigate(['dishes']);
          return this.notificationService.showSuccessToast('Changes saved', 'Dish have been successfully saved.');
        }),
      ),
    {
      dispatch: false,
    },
  );

  saveDishFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.saveDishRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during saving a dish.')),
      ),
    {
      dispatch: false,
    },
  );

  editDishRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.editDishRequest),
      exhaustMap(({ payload }) => {
        return this.dishService.editDish(payload.dish).pipe(
          switchMap(() => of(DishActions.editDishRequestSuccess({ dishName: payload.dish.name }))),
          catchError(error => of(DishActions.editDishRequestFailed(error))),
        );
      }),
    ),
  );

  editDishSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.editDishRequestSuccess),
        tap(action => {
          this.router.navigate(['dishes']);
          return this.notificationService.showSuccessToast(
            'Changes saved',
            `Dish "${action.payload.dishName}" have been successfully updated.`,
          );
        }),
      ),
    {
      dispatch: false,
    },
  );

  editDishFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.editDishRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during saving a dish.')),
      ),
    {
      dispatch: false,
    },
  );

  deleteDishRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.deleteDishRequest),
      exhaustMap(({ payload }) => {
        return this.dishService.deleteDish(payload.id).pipe(
          switchMap(() => of(DishActions.deleteDishRequestSuccess(), DishActions.loadDishesRequest())),
          catchError(error => of(DishActions.deleteDishRequestFailed(error))),
        );
      }),
    ),
  );

  deleteDishRequestSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.deleteDishRequestSuccess),
        tap(() => {
          return this.notificationService.showSuccessToast('Changes saved', `Dish has been successfully removed.`);
        }),
      ),
    {
      dispatch: false,
    },
  );

  deleteDishRequestFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.deleteDishRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during removing the dish.')),
      ),
    {
      dispatch: false,
    },
  );

  updateCustomizedPortionRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.updatePortionRequest),
      exhaustMap(({ payload }) => {
        return this.dishService
          .updatePortionMultiplier(payload.dishId, payload.productId, payload.customizedPortionMultiplier)
          .pipe(
            switchMap(() =>
              of(
                DishActions.updatePortionRequestSuccess(),
                MealCalendarActions.getMealsRequest({ date: payload.date }),
              ),
            ),
            catchError(error => of(DishActions.updatePortionRequestFailed(error))),
          );
      }),
    ),
  );
  updatePortionEffectSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.updatePortionRequestSuccess),
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
        ofType(DishActions.updatePortionRequestFailed),
        tap(() => this.notificationService.showErrorToast('Error', 'An error occured during saving portion.')),
      ),
    {
      dispatch: false,
    },
  );
}
