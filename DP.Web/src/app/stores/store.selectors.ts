import { createFeatureSelector, createSelector } from '@ngrx/store';
import { GeneralState } from './store.state';

const getState = createFeatureSelector<GeneralState>('generalState');

export const getMeasurements = createSelector(getState, state => state.measurements);
export const getProducts = createSelector(getState, state => state.products);
export const getErrorCode = createSelector(getState, state => state.errorCode);
