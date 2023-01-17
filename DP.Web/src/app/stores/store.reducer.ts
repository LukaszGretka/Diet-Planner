import {createReducer, on} from '@ngrx/store';
import * as generalActions from './store.actions';
import {GeneralState} from './store.state';

export const initialState: GeneralState = {
  errorCode: null,
  products: null,
  measurements: null,
};

const reducerFactory = createReducer(
  initialState,
  on(generalActions.clearState, state => ({
    ...state,
    errorCode: null,
  })),
  on(generalActions.setErrorCode, (state, action) => ({
    ...state,
    errorCode: action.payload.errorCode == 0 ? 503 : action.payload.errorCode,
  })),
  on(generalActions.getProductsRequestCompleted, (state, action) => ({
    ...state,
    products: action.payload.products,
  })),
  on(generalActions.getMeasurementsCompleted, (state, action) => ({
    ...state,
    measurements: action.payload.measurements,
  })),
);

export function GeneralReducer(
  state: GeneralState = initialState,
  action: generalActions.GeneralActions,
): GeneralState {
  return reducerFactory(state, action);
}
