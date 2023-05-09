import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, EMPTY, of, switchMap, tap } from 'rxjs';
import { Router } from '@angular/router';
import { MeasurementService } from 'src/app/body-profile/services/measurement.service';
import * as GeneralActions from './store.actions';
import { ProductService } from 'src/app/products/services/product.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Injectable()
export class GeneralEffects {
  getMeasurementEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GeneralActions.getMeasurementsRequest),
      switchMap(() => {
        return this.measurementService.getMeasurements().pipe(
          switchMap(payload => {
            return of(GeneralActions.getMeasurementsCompleted({ measurements: payload }));
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
      ofType(GeneralActions.addMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.addMeasurement(payload.measurementData).pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(GeneralActions.addMeasurementRequestCompleted());
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
      ofType(GeneralActions.editMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.editMeasurement(payload.measurementId, payload.measurementData).pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(GeneralActions.editMeasurementRequestCompleted());
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
      ofType(GeneralActions.removeMeasurementRequest),
      switchMap(({ payload }) => {
        return this.measurementService.deleteMeasurement(payload.measurementId).pipe(
          switchMap(() => {
            window.location.reload();
            return EMPTY;
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  getProductsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GeneralActions.getProductsRequest),
      switchMap(() => {
        return this.productService.getProducts().pipe(
          switchMap(products => of(GeneralActions.getProductsRequestCompleted({ products }))),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  addProductEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GeneralActions.addProductRequest),
      switchMap(({ payload }) => {
        return this.productService.addProduct(payload.productData).pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(GeneralActions.addProductRequestCompleted());
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  removeProductEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GeneralActions.removeProductRequest),
      switchMap(({ payload }) => {
        return this.productService.removeProduct(payload.productId).pipe(
          switchMap(() => {
            window.location.reload();
            return EMPTY;
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  editProductEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(GeneralActions.editProductRequest),
      switchMap(({ payload }) => {
        return this.productService.editProduct(payload.productId, payload.productData).pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(GeneralActions.editProductRequestCompleted());
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

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

  constructor(
    private actions$: Actions,
    private router: Router,
    private measurementService: MeasurementService,
    private productService: ProductService,
    private notificationService: NotificationService,
  ) {}
}
