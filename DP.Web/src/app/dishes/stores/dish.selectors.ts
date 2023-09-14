import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DishState } from './dish.state';

const getState = createFeatureSelector<DishState>('dishState');

export const isLoading = createSelector(getState, state => state.isLoading);
export const getDishes = createSelector(getState, state => state.dishes);
