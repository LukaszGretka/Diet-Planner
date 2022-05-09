import { createFeatureSelector, createSelector } from "@ngrx/store";
import { GeneralState } from "./store.state";

const getState = createFeatureSelector<GeneralState>('generalState');

export const getMeasurementData = createSelector(
    getState,
    state => state.measurementData
);

export const getError = createSelector(
    getState,
    state => state.error
);

export const getProductData = createSelector(
    getState,
    state => state.productData
);

export const getProcessingProductId = createSelector(
    getState,
    state => state.processingProductId
);

