import { createFeatureSelector, createSelector } from '@ngrx/store';
import { MealCalendarState } from './meals-calendar.state';
import { MealType } from '../models/meal-type';
import { Dish } from '../../dishes/models/dish';

const getState = createFeatureSelector<MealCalendarState>('mealCalendarState');

export const getAllDailyMeals = createSelector(getState, state => state?.allDailyMeals);
export const getMealDishById = (id: number) =>
  createSelector(
    getState,
    (state) => {
      for (const meal of state.allDailyMeals) {
        const foundDish = meal.dishes?.find(dish => dish.id === id);
        if (foundDish) return foundDish;
      }
      return null;
    }
  );

export const getMealProductById = (id: number) =>
  createSelector(
    getState,
    (state) => {
      for (const meal of state.allDailyMeals) {
        const foundProduct = meal.products?.find(product => product.id === id);
        if (foundProduct) return foundProduct;
      }
      return null;
    }
  );
