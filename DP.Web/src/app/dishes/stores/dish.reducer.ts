import { createReducer, on } from '@ngrx/store';
import { DishState } from './dish.state';
import * as dishActions from './dish.actions';

export const initialState: DishState = {
  isLoading: false,
};

const reducerFactory = createReducer(
  initialState,
  on(dishActions.saveDishRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(dishActions.saveDishRequestSuccess, state => ({
    ...state,
    isLoading: false,
  })),
  on(dishActions.saveDishRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
);

export function DishReducer(state: DishState = initialState, action: dishActions.DishActions): DishState {
  return reducerFactory(state, action);
}
