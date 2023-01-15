import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { catchError, EMPTY, of, switchMap } from "rxjs";
import { Router } from "@angular/router";
import { MeasurementService } from "src/app/body-profile/services/measurement.service";
import * as GeneralActions from './store.actions';
import { ProductService } from "src/app/products/services/product.service";

@Injectable()
export class GeneralEffects {

  addMeasurementEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.addMeasurementRequest),
    switchMap(({ payload }) => {
      return this.measurementService.addMeasurement(payload.measurementData)
        .pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(GeneralActions.addMeasurementRequestCompleted());
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          }))
    })));

  editMeasurementEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.editMeasurementRequest),
    switchMap(({ payload }) => {
      return this.measurementService.editMeasurement(payload.measurementId, payload.measurementData)
        .pipe(
          switchMap(() => {
            this.router.navigate(['body-profile']);
            return of(GeneralActions.editMeasurementRequestCompleted());
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          })
        )
    })
  ))

  removeMeasurementEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.removeMeasurementRequest),
    switchMap(({ payload }) => {
      return this.measurementService.deleteMeasurement(payload.measurementId)
        .pipe(
          switchMap(() => {
            window.location.reload();
            return EMPTY;
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          }))
    })));

  addProductEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.addProductRequest),
    switchMap(({ payload }) => {
      return this.productService.addProduct(payload.productData)
        .pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(GeneralActions.addProductRequestCompleted());
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          }))
    })));

  removeProductEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.removeProductRequest),
    switchMap(({ payload }) => {
      return this.productService.removeProduct(payload.productId)
        .pipe(
          switchMap(() => {
            window.location.reload();
            return EMPTY;
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          }))
    })));

  editProductEffect$ = createEffect(() => this.actions$.pipe(
    ofType(GeneralActions.editProductRequest),
    switchMap(({ payload }) => {
      return this.productService.editProduct(payload.productId, payload.productData)
        .pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(GeneralActions.editProductRequestCompleted());
          }),
          catchError(error => {
            return of(GeneralActions.setError({ message: error }));
          })
        )
    })
  ));

  constructor(private actions$: Actions, private router: Router,
    private measurementService: MeasurementService, private productService: ProductService) {
  }
}
