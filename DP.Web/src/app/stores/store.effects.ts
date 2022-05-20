import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { select, Store } from "@ngrx/store";
import { catchError, EMPTY, of, switchMap, withLatestFrom } from "rxjs";
import * as GeneralActions from './store.actions';
import { GeneralState } from "./store.state";
import { getMeasurementData, getProductData, getProcessingProductId } from "./store.selectors";
import { Measurement } from "src/models/measurement";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";
import { Product } from "src/models/product";

@Injectable()
export class GeneralEffects {

    private measurementUrl = 'http://localhost:5000/api/measurement';
    private productsUrl = 'http://localhost:5000/api/product';
 
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    };

    addMeasurementEffect$ = createEffect(() => this.actions$.pipe(
        ofType(GeneralActions.submitMeasurementRequest),
        withLatestFrom(this.store.pipe(select(getMeasurementData))),
        switchMap(([_, measurementData]) => {
            return this.httpClient.post<Measurement>(this.measurementUrl, measurementData, this.httpOptions)
                .pipe(
                    switchMap(() => {
                        this.router.navigate(['body-profile']);
                        return of(GeneralActions.submitMeasurementRequestSuccess());
                    }),
                    catchError(error => {
                        return of(GeneralActions.setError({ message: error }));
                    }))
        })));

        addProductEffect$ = createEffect(() => this.actions$.pipe(
            ofType(GeneralActions.submitAddProductRequest),
            withLatestFrom(this.store.pipe(select(getProductData))),
            switchMap(([_, productData]) => {
                return this.httpClient.post<Product>(this.productsUrl, productData, this.httpOptions)
                    .pipe(
                        switchMap(() => {
                            this.router.navigate(['products']);
                            return of(GeneralActions.submitAddProductRequestSuccess());
                        }),
                        catchError(error => {
                            return of(GeneralActions.setError({ message: error }));
                        }))
            })));

        removeProductEffect$ = createEffect(() => this.actions$.pipe(
            ofType(GeneralActions.submitRemoveProductRequest),
            withLatestFrom(this.store.pipe(select(getProcessingProductId))),
            switchMap(([_, productId]) => {
                return this.httpClient.delete<Product>(this.productsUrl + "/" + productId, this.httpOptions)
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
                ofType(GeneralActions.submitEditProductRequest),
                withLatestFrom(this.store.pipe(select(getProductData))),
                switchMap(([_, productData]) => {
                    return this.httpClient.put<Product>(this.productsUrl + '/' + productData.id, productData, this.httpOptions)
                    .pipe(
                        switchMap(() =>{
                            this.router.navigate(['products']);
                            return of(GeneralActions.submitEditProductRequestSuccess());
                        }),
                        catchError(error => {
                            return of(GeneralActions.setError({message: error}));
                        })
                    )
                })
            ))

    constructor(private actions$: Actions, private store: Store<GeneralState>, private httpClient: HttpClient,
        private router: Router) {
    }
}