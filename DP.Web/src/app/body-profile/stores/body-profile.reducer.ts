import { BodyProfileState } from './body-profile.state';
import * as bodyProfileActions from './body-profile.actions';
import { createReducer, on } from '@ngrx/store';

export const initialState: BodyProfileState = {
  measurements: null,
  userProfile: null
};

const reducerFactory = createReducer(
  initialState,
  on(bodyProfileActions.getMeasurementsCompleted, (state, action) => ({
    ...state,
    measurements: action.payload.measurements,
  })),
  on(bodyProfileActions.getUserProfileSuccess, (state, action) => ({
    ...state,
    userProfile: action.payload.userProfile,
  })),
);

export function BodyProfileReducer(
  state: BodyProfileState = initialState,
  action: bodyProfileActions.BodyProfileActions,
): BodyProfileState {
  return reducerFactory(state, action);
}
