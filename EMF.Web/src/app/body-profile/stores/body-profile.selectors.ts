import { createFeatureSelector, createSelector } from "@ngrx/store";
import { BodyProfileState } from "./body-profile.state";

const getState = createFeatureSelector<BodyProfileState>('bodyProfile');

export const getMeasurementData = createSelector(
    getState,
    state => state.measurementData
)

export const getAddMeasurementError = createSelector(
    getState,
    state => state.addMeasurementError
)
