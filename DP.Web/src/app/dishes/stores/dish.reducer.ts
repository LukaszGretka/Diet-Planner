import { createReducer } from '@ngrx/store';
import { DishState } from './dish.state';
import * as dishActions from './dish.actions';

export const initialState: DishState = {};

const reducerFactory = createReducer(initialState);

export function DishReducer(state: DishState = initialState, action: dishActions.DishActions): DishState {
  return reducerFactory(state, action);
}
