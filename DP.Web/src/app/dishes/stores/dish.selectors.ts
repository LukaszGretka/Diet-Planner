import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DishState } from './dish.state';

const getState = createFeatureSelector<DishState>('dishState');

export const isLoading = createSelector(getState, state => state.isLoading);
export const getCallbackMealDish = createSelector(getState, state => state.callbackMealDish);
export const getDishes = createSelector(getState, state => state.dishes);
export const getDishById = (id: number) =>
  createSelector(getState, state => state.dishes.filter(dish => dish.id === id)[0]);
