import { Injectable, inject } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DishService } from '../services/dish.service';
import { catchError, exhaustMap, of, switchMap, tap, withLatestFrom } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as DishActions from './dish.actions';
import * as DishSelectors from './dish.selectors';
import { Router } from '@angular/router';
import { DishState } from './dish.state';
import { Store } from '@ngrx/store';

@Injectable()
export class DishEffects {
  private readonly actions$ = inject(Actions);
  private readonly dishService = inject(DishService);
  private readonly notificationService = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly dishStore = inject<Store<DishState>>(Store);

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

  getDishByNameRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.getDishByNameRequest),
      exhaustMap(({ payload }) => {
        return this.dishService.getDishByName(payload.name).pipe(
          switchMap(() => of(DishActions.getDishByNameRequestSuccess())),
          catchError(error => of(DishActions.getDishByNameRequestFailed(error))),
        );
      }),
    ),
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
}
