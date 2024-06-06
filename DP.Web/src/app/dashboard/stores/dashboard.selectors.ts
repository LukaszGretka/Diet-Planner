import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DashboardState } from './dashboard.state';

const getState = createFeatureSelector<DashboardState>('dashboardState');

export const getDashboardData = createSelector(getState, state => state.dashboardData);
export const getErrorCode = createSelector(getState, state => state.errorCode);
