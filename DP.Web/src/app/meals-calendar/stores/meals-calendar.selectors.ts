import { createFeatureSelector, createSelector } from '@ngrx/store';
import { MealCalendarState } from './meals-calendar.state';
import { ItemType } from 'src/app/shared/models/base-item';

const getState = createFeatureSelector<MealCalendarState>('mealCalendarState');

export const getAllDailyMeals = createSelector(getState, state => state?.allDailyMeals);
export const getMealDishById = (id: number) =>
  createSelector(getState, state => {
    for (const meal of state.allDailyMeals) {
      const foundDish = meal.dishes?.find(dish => dish.mealItemId === id);
      if (foundDish) return foundDish;
    }
    return null;
  });

export const getMealProductById = (id: number) =>
  createSelector(getState, state => {
    for (const meal of state.allDailyMeals) {
      const foundProduct = meal.products?.find(product => product.mealItemId === id);
      if (foundProduct) return foundProduct;
    }
    return null;
  });

export const getMealItemById = (id: number, itemType: ItemType) =>
  createSelector(getState, state => {
    if (itemType === ItemType.Dish) {
      return getMealDishById(id)(state);
    }
    if (itemType === ItemType.Product) {
      return getMealProductById(id)(state);
    }
    return null;
  });
