import { createReducer, on } from "@ngrx/store";
import * as generalActions from "./store.actions";
import { GeneralState } from "./store.state";

export const initialState: GeneralState = {
  error: '',
};

const reducerFactory = createReducer(
  initialState,
  on(generalActions.clearState,
    (state) => ({
      ...state,
      error: '',
    })),
  on(generalActions.setError,
    (state, action) => ({
      ...state,
      error: action.payload.message
    }))
)

export function GeneralReducer(
  state: GeneralState = initialState,
  action: generalActions.GeneralActions): GeneralState {
  return reducerFactory(state, action);
}
