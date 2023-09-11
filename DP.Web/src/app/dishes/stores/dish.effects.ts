import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DishService } from '../services/dish.service';
import { catchError, exhaustMap, of, switchMap, tap } from 'rxjs';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as DishActions from './dish.actions';

@Injectable()
export class DishEffects {
  constructor(
    private actions$: Actions,
    private dishService: DishService,
    private notificationService: NotificationService,
  ) {
    //...
  }

  saveDishEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.saveDishRequest),
      exhaustMap(({ payload }) => {
        return this.dishService.saveDish(payload.dish).pipe(
          switchMap(() => of(DishActions.saveDishRequestSuccess())),
          catchError(() => of(DishActions.saveDishRequestFailed())),
        );
      }),
    ),
  );

  updatePortionEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DishActions.updatePortionRequest),
      exhaustMap(({ payload }) => {
        return this.dishService
          .updatePortionMultiplier(payload.dishId, payload.productId, payload.portionMultiplier)
          .pipe(
            switchMap(() => of(DishActions.updatePortionRequestSuccess())),
            catchError(() => of(DishActions.updatePortionRequestFailed())),
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
