import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY, catchError, of, switchMap } from 'rxjs';
import { MeasurementService } from '../services/measurement.service';
import * as BodyProfileActions from './body-profile.actions';
import * as GeneralActions from './../../stores/store.actions';

@Injectable()
export class BodyProfileEffects {
  getMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.getMeasurementsRequest),
      switchMap(() => {
        return this.measurementService.getMeasurements().pipe(
          switchMap(payload => {
            return of(BodyProfileActions.getMeasurementsCompleted({ measurements: payload }));
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  addMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.addMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.addMeasurement(payload.measurementData).pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(BodyProfileActions.addMeasurementRequestCompleted());
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  editMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.editMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.editMeasurement(payload.measurementId, payload.measurementData).pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(BodyProfileActions.editMeasurementRequestCompleted());
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  removeMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.removeMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.deleteMeasurement(payload.measurementId).pipe(
          switchMap(() => {
            window.location.reload(); //TODO: find better way
            return EMPTY;
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  constructor(private actions$: Actions, private router: Router, private measurementService: MeasurementService) {}
}
