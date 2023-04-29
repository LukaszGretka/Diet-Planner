import {Injectable} from '@angular/core';
import {Actions, createEffect, ofType} from '@ngrx/effects';
import {EMPTY, catchError, exhaustMap, of, switchMap, tap} from 'rxjs';
import {AccountService} from '../services/account.service';
import * as AccountActions from './account.actions';
import {Router} from '@angular/router';
import {NotificationService} from 'src/app/shared/services/notification.service';

@Injectable()
export class AccountEffects {
  getUserEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.getUserRequest),
      exhaustMap(() => {
        return this.accountService.getUserClaims().pipe(
          switchMap(user => {
            this.accountService.setUser(user);
            return of(AccountActions.getUserRequestSuccess({user}));
          }),
          catchError(error => of(AccountActions.getUserRequestFailed({error}))),
        );
      }),
    ),
  );

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
          this.notificationService.showSuccessToast(
            'Successfully signed in.',
            `Welcome ${action.payload.signInResult.user.username}!`,
          );
          this.accountService.authenticatedUser$.next(action.payload.signInResult.user);
          this.router.navigate([action.payload.signInResult.returnUrl ?? 'dashboard']);
        }),
      ),
    {dispatch: false},
  );

  signInRequestFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signInRequestFailed),
        tap(action => {
          if ((action.payload.error as any).status == 0) {
            this.notificationService.showGenericErrorToast(0);
          } else this.notificationService.showErrorToast('Sign in error.', 'Invalid credentials.');
        }),
      ),
    {dispatch: false},
  );

  signUpRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signUpRequest),
      exhaustMap(({payload}) => {
        return this.accountService.performSignUp(payload.signUpRequest).pipe(
          switchMap(signUpResult => of(AccountActions.signUpSuccess({signUpResult}))),
          catchError(error => of(AccountActions.signUpRequestFailed({error}))),
        );
      }),
    ),
  );

  signUpRequestFailedEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signUpRequestFailed),
        tap(() => {
          this.notificationService.showErrorToast('Sign up error.', 'Unable to sign up new user.');
        }),
      ),
    {dispatch: false},
  );

  signUpSuccessEffect$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AccountActions.signUpSuccess),
        tap(action => {
          this.notificationService.showSuccessToast(
            'Successfully signed up.',
            'Hello ' + action.payload.signUpResult.user.username,
          );
          this.accountService.authenticatedUser$.next(action.payload.signUpResult.user);
          this.router.navigate(['/dashboard']);
        }),
      ),
    {dispatch: false},
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
        tap(() => {
          this.notificationService.showSuccessToast('Successfully signed out.', 'See you soon!');
          this.accountService.authenticatedUser$.next(null);
          this.router.navigate(['/sign-in']);
        }),
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
