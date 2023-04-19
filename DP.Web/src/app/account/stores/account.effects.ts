import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {EMPTY, catchError, exhaustMap, of, switchMap, tap} from 'rxjs';
import {AccountService} from '../services/account.service';
import * as AccountActions from './account.actions';
import {Router} from '@angular/router';
import {NotificationService} from 'src/app/shared/services/notification.service';

@Injectable()
export class AccountEffects {
  signInRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signInRequest),
      exhaustMap(({payload}) => {
        return this.accountService.performSignIn(payload.signInRequest).pipe(
          switchMap(signInResult => of(AccountActions.signInRequestSuccess({signInResult}))),
          catchError(error => of(AccountActions.signInRequestFailed({error}))),
        );
      }),
    ),
  );

  signInRequestSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signInRequestSuccess),
        tap(action => {
          this.accountService.authenticatedUser$.next(action.payload.signInResult.user);
          this.notificationService.showSuccessToast(
            'Successfully signed in.',
            `Welcome ${action.payload.signInResult.user.username}!`,
          );
          this.router.navigate([action.payload.signInResult.returnUrl]);
        }),
      ),
    {dispatch: false},
  );

  signInRequestFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signInRequestFailed),
        tap(() => {
          this.notificationService.showErrorToast('Sign in error.', 'Invalid credentials.');
        }),
      ),
    {dispatch: false},
  );

  signUpRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signUpRequest),
      exhaustMap(({payload}) => {
        return this.accountService.performSignUp(payload.signUpRequest).pipe(
          switchMap(user => of(AccountActions.signUpSuccess({user}))),
          catchError(error => of(AccountActions.signUpRequestFailed({error}))),
        );
      }),
    ),
  );

  signoutRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signOutRequest),
      exhaustMap(() => {
        return this.accountService.performSignOut().pipe(
          switchMap(() => of(AccountActions.signOutRequestSuccess())),
          catchError(error => of(AccountActions.signOutRequestFailed({error}))),
        );
      }),
    ),
  );

  signoutRequestSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signOutRequestSuccess),
        tap(() => this.router.navigate(['/sign-in'])),
      ),
    {dispatch: false},
  );

  constructor(
    private actions$: Actions,
    private accountService: AccountService,
    private router: Router,
    private notificationService: NotificationService,
  ) {}
}
