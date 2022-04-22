import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { select, Store } from "@ngrx/store";
import { catchError, of, switchMap, withLatestFrom } from "rxjs";
import * as BodyProfileActions from '../stores/body-profile.actions';
import { BodyProfileState } from "./body-profile.state";
import { getMeasurementData } from "./body-profile.selectors";
import { Measurement } from "src/models/measurement";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";

@Injectable()
export class BodyProfileEffects {

    private measurementUrl = 'http://localhost:5000/api/measurement';

    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    };

    submitAddMeasurement$ = createEffect(() => this.actions$.pipe(
        ofType(BodyProfileActions.submitMeasurementRequest),
        withLatestFrom(this.store.pipe(select(getMeasurementData))),
        switchMap(([_, measurementData]) => {
            return this.httpClient.post<Measurement>(this.measurementUrl, measurementData, this.httpOptions)
                .pipe(
                    switchMap(() => {
                        this.router.navigate(['body-profile']);
                        return of(BodyProfileActions.submitMeasurementRequestSuccess);
                    }),
                    catchError(error => {
                        return of(BodyProfileActions.setError({ message: error }));
                    }))
        })));


    constructor(private actions$: Actions, private store: Store<BodyProfileState>, private httpClient: HttpClient,
        private router: Router) {
    }
}