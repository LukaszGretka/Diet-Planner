import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY, catchError, exhaustMap, of, switchMap } from 'rxjs';
import { ProductService } from '../services/product.service';
import * as ProductsActions from './products.actions';
import * as GeneralActions from './../../stores/store.actions';
import { Product } from '../models/product';
import { Router } from '@angular/router';

@Injectable()
export class ProductsEffects {
  constructor(private actions$: Actions, private productService: ProductService, private router: Router) {}

  addProductEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.addProductRequest),
      exhaustMap(({ payload }) => {
        return this.productService.addProduct(payload.productData).pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(ProductsActions.addProductRequestCompleted());
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
      ofType(ProductsActions.removeProductRequest),
      exhaustMap(({ payload }) => {
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
      ofType(ProductsActions.editProductRequest),
      exhaustMap(({ payload }) => {
        return this.productService.editProduct(payload.productId, payload.productData).pipe(
          switchMap(() => {
            this.router.navigate(['products']);
            return of(ProductsActions.editProductRequestCompleted());
          }),
          catchError((error: any) =>
            of(GeneralActions.setErrorCode({ errorCode: error.status, errorMessage: error.error.message })),
          ),
        );
      }),
    ),
  );

  getProductByNameEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.getProductByNameRequest),
      exhaustMap(({ payload }) => {
        return this.productService.getProductByName(payload.name).pipe(
          switchMap((result: Product) => of(ProductsActions.getProductByNameRequestSuccess({ result }))),
          catchError((error: any) => of(ProductsActions.getProductByNameRequestFailed({ errorCode: error.status }))),
        );
      }),
    ),
  );

  getAllProductsEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.getAllProductsRequest),
      exhaustMap(() => {
        return this.productService.getProductsWithPortion().pipe(
          switchMap((result: Product[]) => of(ProductsActions.getAllProductsRequestSuccess({ result }))),
          catchError((error: any) => of(ProductsActions.getAllProductsRequestFailed({ errorCode: error.status }))),
        );
      }),
    ),
  );
}
