import { createFeatureSelector, createSelector } from '@ngrx/store';
import { GeneralState } from './store.state';

const getState = createFeatureSelector<GeneralState>('generalState');

export const getErrorCode = createSelector(getState, state => state.errorCode);
