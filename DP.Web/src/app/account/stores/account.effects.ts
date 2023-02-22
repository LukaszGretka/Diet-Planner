import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, exhaustMap, of, switchMap } from "rxjs";
import { AccountService } from "../services/account.service";
import * as AccountActions from "./account.actions";

@Injectable()
export class AccountEffects {
  logInRequestEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AccountActions.logInRequest),
      exhaustMap(({ payload }) => {
        return this.accountService.performLogIn(payload.logInRequest).pipe(
          switchMap((user) => of(AccountActions.logInRequestSuccess({ user }))),
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

  constructor(
    private actions$: Actions,
    private accountService: AccountService
  ) { }
}
