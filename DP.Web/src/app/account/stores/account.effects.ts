import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, exhaustMap, of, switchMap, tap } from "rxjs";
import { AccountService } from "../services/account.service";
import * as AccountActions from "./account.actions";
import { Router } from "@angular/router";

@Injectable()
export class AccountEffects {
  logInRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.logInRequest),
      exhaustMap(({ payload }) => {
        return this.accountService.performLogIn(payload.logInRequest).pipe(
          switchMap((logInResult) => of(AccountActions.logInRequestSuccess({ logInResult }))),
          catchError((error) => of(AccountActions.logInRequestFailed({ error })))
        )
      })
    )
  )

  signUpRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signUpRequest),
      exhaustMap(({ payload }) => {
        return this.accountService.performSignUp(payload.signUpRequest).pipe(
          switchMap((user) => of(AccountActions.signUpSuccess({ user }))),
          catchError((error) => of(AccountActions.signUpRequestFailed({ error })))
        )
      })
    )
  )

  logInRequestSuccessEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.logInRequestSuccess),
      tap((action) => {
        this.accountService.authenticatedUser$.next(action.payload.logInResult.user);
        this.router.navigate([action.payload.logInResult.returnUrl])
      })
    ),
    { dispatch: false }
  )

  signoutRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signOutRequest),
      exhaustMap(() => {
        return this.accountService.performSignOut().pipe(
          switchMap(() => of(AccountActions.signOutRequestSuccess())),
          catchError((error) => of(AccountActions.signOutRequestFailed({ error })))
        )
      })
    ));

  signoutRequestSuccessEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.signOutRequestSuccess),
      tap(() => this.router.navigate(['/log-in'])),
    ),
    { dispatch: false })
    ;

  constructor(
    private actions$: Actions,
    private accountService: AccountService,
    private router: Router
  ) { }
}
