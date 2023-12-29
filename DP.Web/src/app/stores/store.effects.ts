import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of, tap } from 'rxjs';
import * as GeneralActions from './store.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Injectable()
export class GeneralEffects {
  showNotificationToastOnErrorEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(GeneralActions.setErrorCode),
        tap(error => {
          return of(
            this.notificationService.showErrorToast('Error', error.payload.errorMessage ?? 'An error occurred.'),
          );
        }),
      ),
    { dispatch: false },
  );

  constructor(private actions$: Actions, private notificationService: NotificationService) {}
}
