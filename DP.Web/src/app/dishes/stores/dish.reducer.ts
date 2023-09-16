import { createReducer, on } from '@ngrx/store';
import { DishState } from './dish.state';
import * as dishActions from './dish.actions';

export const initialState: DishState = {
  isLoading: false,
  dishes: [],
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
  on(dishActions.editDishRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(dishActions.editDishRequestSuccess, state => ({
    ...state,
    isLoading: false,
  })),
  on(dishActions.editDishRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
  on(dishActions.loadDishesRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(dishActions.loadDishesRequestSuccess, (state, action) => ({
    ...state,
    isLoading: false,
    dishes: action.payload.dishes,
  })),
  on(dishActions.loadDishesRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
);

export function DishReducer(state: DishState = initialState, action: dishActions.DishActions): DishState {
  return reducerFactory(state, action);
}
