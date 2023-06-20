import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, of, switchMap } from 'rxjs';
import { ProductService } from '../services/product.service';
import * as ProductsActions from './products.actions';
import { Product } from '../models/product';

@Injectable()
export class ProductsEffects {
  constructor(private actions$: Actions, private productService: ProductService) {}

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
}
