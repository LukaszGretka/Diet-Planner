import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DishService } from '../services/dish.service';
import { catchError, exhaustMap, of, switchMap, tap, withLatestFrom } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as DishActions from './dish.actions';
import * as DishSelectors from './dish.selectors';
import * as MealCalendarActions from './../../meals-calendar/stores/meals-calendar.actions';
import { Router } from '@angular/router';
import { DishState } from './dish.state';
import { Store } from '@ngrx/store';

@Injectable()
export class DishEffects {
  constructor(
    private actions$: Actions,
    private dishService: DishService,
    private notificationService: NotificationService,
    private router: Router,
    private dishStore: Store<DishState>,
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
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during loading dishes.')),
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
          withLatestFrom(this.dishStore.select(DishSelectors.getCallbackMealDish)),
          switchMap(([_, callbackMealDish]) => {
            return of(DishActions.saveDishRequestSuccess({ callbackMealDish }));
          }),
          catchError(error => of(DishActions.saveDishRequestFailed(error))),
        );
      }),
    ),
  );

  saveDishSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(DishActions.saveDishRequestSuccess),
        tap(action => {
          if (action.payload.callbackMealDish) {
            this.router.navigateByUrl('meals-calendar');
          } else {
            this.router.navigate(['dishes']);
          }
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
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during saving a dish.')),
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
          switchMap(() =>
            of(DishActions.editDishRequestSuccess({ dishName: payload.dish.name, returnUrl: payload.returnUrl })),
          ),
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
          if (action.payload.returnUrl) {
            this.router.navigateByUrl(action.payload.returnUrl);
          } else {
            this.router.navigate(['dishes']);
          }
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
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during saving a dish.')),
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
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during removing the dish.')),
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
          .updatePortionMultiplier(payload.dishId, payload.productId, payload.mealDishId, payload.customizedPortionMultiplier)
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
        tap(() => this.notificationService.showErrorToast('Error', 'An error occurred during saving portion.')),
      ),
    {
      dispatch: false,
    },
  );
}
