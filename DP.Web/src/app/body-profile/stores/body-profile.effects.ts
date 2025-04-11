import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY, catchError, exhaustMap, of, switchMap } from 'rxjs';
import { MeasurementService } from '../services/measurement.service';
import * as BodyProfileActions from './body-profile.actions';
import * as GeneralActions from './../../stores/store.actions';
import { UserProfileService } from '../services/user-profile.service';
import { UserProfile } from '../models/user-profile';

@Injectable()
export class BodyProfileEffects {
  private readonly actions$ = inject(Actions);
  private readonly router = inject(Router);
  private readonly measurementService = inject(MeasurementService);
  private readonly userProfileService = inject(UserProfileService);

  getMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.getMeasurementsRequest),
      exhaustMap(() => {
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
      exhaustMap(({ payload }) => {
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
      exhaustMap(({ payload }) => {
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
      exhaustMap(({ payload }) => {
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

  getUserProfileEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.getUserProfileRequest),
      exhaustMap(_ => {
        return this.userProfileService.getUserProfile().pipe(
          switchMap((userProfile: UserProfile) => of(BodyProfileActions.getUserProfileSuccess({ userProfile }))),
          catchError((error: any) => of(BodyProfileActions.getUserProfileFailed({ error: error.error.message }))),
        );
      }),
    ),
  );

  updateUserProfileEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.updateUserProfileRequest),
      exhaustMap(({ payload }) => {
        return this.userProfileService.updateUserProfile(payload.userProfile).pipe(
          switchMap(() => {
            window.location.reload(); //TODO: find better way
            return EMPTY;
          }),
          catchError((error: any) => of(BodyProfileActions.updateUserProfileFailed({ error: error.error.message }))),
        );
      }),
    ),
  );

  uploadUserAvatarEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(BodyProfileActions.uploadUserAvatarRequest),
      exhaustMap(({ payload }) => {
        return this.userProfileService.updateUserAvatar(payload.base64Avatar).pipe(
          switchMap((userProfile: UserProfile) =>
            of(BodyProfileActions.uploadUserAvatarRequestSuccess({ userProfile })),
          ),
          catchError((error: any) =>
            of(BodyProfileActions.uploadUserAvatarRequestFailed({ error: error.error.message })),
          ),
        );
      }),
    ),
  );
}
