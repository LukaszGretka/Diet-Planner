import { createReducer, on } from '@ngrx/store';
import * as generalActions from './store.actions';
import { GeneralState } from './store.state';

export const initialState: GeneralState = {
  errorCode: null
};

const reducerFactory = createReducer(
  initialState,
  on(generalActions.setErrorCode, (state, action) => ({
    ...state,
    errorCode: action.payload.errorCode == 0 ? 503 : action.payload.errorCode,
  })),
  on(generalActions.clearErrors, state => ({
    ...state,
    errorCode: null,
  })),
);

export function GeneralReducer(
  state: GeneralState = initialState,
  action: generalActions.GeneralActions,
): GeneralState {
  return reducerFactory(state, action);
}
