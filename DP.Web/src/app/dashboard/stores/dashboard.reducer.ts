import { createReducer, on } from '@ngrx/store';
import * as dashboardActions from './dashboard.actions';
import { DashboardState } from './dashboard.state';

export const initialState: DashboardState = {
  dashboardData: null,
  errorCode: null
};

const reducerFactory = createReducer(
  initialState,
  on(dashboardActions.getDashboardDataSuccess, (state, action) => ({
    ...state,
    dashboardData: action.payload.data
  })),
  on(dashboardActions.getDashboardDataFailed, (state, action) => ({
    ...state,
    errorCode: action.payload.errorCode == 0 ? 503 : action.payload.errorCode,
  })),
);

export function DashboardReducer(
  state: DashboardState = initialState,
  action: dashboardActions.DashboardActions,
): DashboardState {
  return reducerFactory(state, action);
}
