import { createFeatureSelector, createSelector } from '@ngrx/store';
import { BodyProfileState } from './body-profile.state';

const getState = createFeatureSelector<BodyProfileState>('bodyProfileState');

export const getMeasurements = createSelector(getState, state => state.measurements);

export const getUserProfile = createSelector(getState, state => state.userProfile);
