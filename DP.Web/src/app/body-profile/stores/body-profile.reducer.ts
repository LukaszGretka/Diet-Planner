import { BodyProfileState } from './body-profile.state';
import * as bodyProfileActions from './body-profile.actions';
import { createReducer, on } from '@ngrx/store';

export const initialState: BodyProfileState = {
  measurements: null,
};

const reducerFactory = createReducer(
  initialState,
  on(bodyProfileActions.getMeasurementsCompleted, (state, action) => ({
    ...state,
    measurements: action.payload.measurements,
  })),
);

export function BodyProfileReducer(
  state: BodyProfileState = initialState,
  action: bodyProfileActions.BodyProfileActions,
): BodyProfileState {
  return reducerFactory(state, action);
}
