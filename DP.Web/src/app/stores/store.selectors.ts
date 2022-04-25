import { createFeatureSelector, createSelector } from "@ngrx/store";
import { GeneralState } from "./store.state";

const getState = createFeatureSelector<GeneralState>('generalState');

export const getMeasurementData = createSelector(
    getState,
    state => state.measurementData
);

export const getAddMeasurementError = createSelector(
    getState,
    state => state.addMeasurementError
);

export const getProductData = createSelector(
    getState,
    state => state.productData
);

