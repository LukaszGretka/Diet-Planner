import { createAction, union } from '@ngrx/store';
import { DailyMealsOverview } from '../models/daily-meals-overview';
import { Meal, MealByDay } from '../models/meal';

export const getMealsRequest = createAction('Get meals request', prop<{ date: Date }>());
export const getMealsRequestSuccess = createAction('Get meals request success', prop<{ result: Meal[] }>());
export const getMealsRequestFailed = createAction('Get meals request failed', prop<{ error: string }>());

export const addMealRequest = createAction('Add meal by day', prop<{ mealByDay: MealByDay }>());
export const addMealRequestCompleted = createAction('Add meal by day completed');

const actions = union({
  getMealsRequest,
  getMealsRequestSuccess,
  getMealsRequestFailed,
  addMealRequest,
  addMealRequestCompleted,
});

export type MealCalendarActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
