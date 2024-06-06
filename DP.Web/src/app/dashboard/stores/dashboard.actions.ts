import { createAction, union } from '@ngrx/store';
import { DashboardData } from '../models/dashboard-data';

export const setErrorCode = createAction('Set error code', prop<{ errorCode: number | null; errorMessage?: string }>());
export const clearErrors = createAction('Clear errors');

export const getDashboardDataRequest = createAction('Get Dashboard Data Request');
export const getDashboardDataSuccess = createAction('Get Dashboard Data Success',
  prop<{ data: DashboardData }>());
export const getDashboardDataFailed = createAction('Get Dashboard Data Failed',
  prop<{ errorCode: number | null; errorMessage?: string }>());

const actions = union({
  setErrorCode,
  clearErrors,
  getDashboardDataRequest,
  getDashboardDataSuccess,
  getDashboardDataFailed
});

export type DashboardActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
