import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, of, switchMap, tap } from 'rxjs';
import * as DashboardActions from './dashboard.actions';
import { Router } from '@angular/router';
import { DashboardService } from '../services/dashboard.service';

@Injectable()
export class DashboardEffects {

  getDashboardDataEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(DashboardActions.getDashboardDataRequest),
      exhaustMap(() => {
        return this.dashboardService.getDashboardData().pipe(
          switchMap(data => {
            return of(DashboardActions.getDashboardDataSuccess({ data }));
          }),
          catchError(error => of(DashboardActions.getDashboardDataFailed({
            errorCode: error.status,
            errorMessage: error.error.message
          }))),
        );
      }),
    ),
  );


  constructor(
    private actions$: Actions,
    private router: Router,
    private dashboardService: DashboardService,
  ) { }
}
