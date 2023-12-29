import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { EMPTY, catchError, exhaustMap, of, switchMap, withLatestFrom } from 'rxjs';
import { ProductService } from '../services/product.service';
import * as ProductsActions from './products.actions';
import * as ProductsSelectors from './products.selectors';
import * as GeneralActions from './../../stores/store.actions';
import { Product } from '../models/product';
import { Router } from '@angular/router';
import { ProductsState } from './products.state';
import { Store } from '@ngrx/store';

@Injectable()
export class ProductsEffects {
  constructor(private actions$: Actions, private productService: ProductService, private router: Router, 
    private productsStore: Store<ProductsState>) {}

  addProductEffect$ = createEffect(() =>
    this.actions$.pipe(
      ofType(ProductsActions.addProductRequest),
      switchMap(({ payload }) => {
        return this.productService.addProduct(payload.productData).pipe(
          withLatestFrom(this.productsStore.select(ProductsSelectors.getCallbackMealProduct)),
          switchMap(([_, callbackMealProduct]) => {
            if (callbackMealProduct) {
              this.router.navigateByUrl('meals-calendar');
            } else {
              this.router.navigate(['products']);
            }
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
      ofType(ProductsActions.editProductRequest),
      switchMap(({ payload }) => {
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
