import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { select, Store } from "@ngrx/store";
import { catchError, of, switchMap, withLatestFrom } from "rxjs";
import * as GeneralActions from './store.actions';
import { GeneralState } from "./store.state";
import { getMeasurementData, getProductData } from "./store.selectors";
import { Measurement } from "src/models/measurement";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable()
export class GeneralEffects {

    private measurementUrl = 'http://localhost:5000/api/measurement';
    private productsUrl = 'http://localhost:5000/api/product';
 

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    };

    submitAddMeasurement$ = createEffect(() => this.actions$.pipe(
        ofType(GeneralActions.submitMeasurementRequest),
        withLatestFrom(this.store.pipe(select(getMeasurementData))),
        switchMap(([_, measurementData]) => {
            return this.httpClient.post<Measurement>(this.measurementUrl, measurementData, this.httpOptions)
                .pipe(
                    switchMap(() => {
                        this.router.navigate(['body-profile']);
                        return of(GeneralActions.submitMeasurementRequestSuccess);
                    }),
                    catchError(error => {
                        return of(GeneralActions.setError({ message: error }));
                    }))
        })));

        submitAddProdct$ = createEffect(() => this.actions$.pipe(
            ofType(GeneralActions.submitAddProductRequest),
            withLatestFrom(this.store.pipe(select(getProductData))),
            switchMap(([_, productData]) => {
                return this.httpClient.post<Measurement>(this.productsUrl, productData, this.httpOptions)
                    .pipe(
                        switchMap(() => {
                            this.router.navigate(['products']);
                            return of(GeneralActions.submitAddProductRequestSuccess);
                        }),
                        catchError(error => {
                            return of(GeneralActions.setError({ message: error }));
                        }))
            })));


    constructor(private actions$: Actions, private store: Store<GeneralState>, private httpClient: HttpClient,
        private router: Router) {
    }
}