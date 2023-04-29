import { createAction, union } from '@ngrx/store';
import { Meal, MealByDay } from '../models/meal';

export const getMealsRequest = createAction('Get meals request', prop<{ date: Date }>());
export const getMealsRequestSuccess = createAction('Get meals request success', prop<{ result: Meal[] }>());
export const getMealsRequestFailed = createAction('Get meals request failed', prop<{ errorCode: number }>());

export const addMealRequest = createAction('Add meal by day request', prop<{ mealByDay: MealByDay }>());
export const addMealRequestSuccess = createAction('Add meal by day success');
export const addMealRequestFailed = createAction('Add meal by day failed', prop<{ errorCode: number }>());

const actions = union({
  getMealsRequest,
  getMealsRequestSuccess,
  getMealsRequestFailed,
  addMealRequest,
  addMealRequestSuccess,
  addMealRequestFailed,
});

export type MealCalendarActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
