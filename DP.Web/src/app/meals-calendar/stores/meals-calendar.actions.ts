import { createAction, union } from '@ngrx/store';
import { AddMealItemRequest, Meal } from '../models/meal';

export const getAllMealsRequest = createAction('Get meals request', prop<{ date: Date }>());
export const getAllMealsRequestSuccess = createAction('Get all meals request success', prop<{ result: Meal[] }>());
export const getAllMealsRequestFailed = createAction('Get all meals request failed', prop<{ errorCode: number }>());

export const addMealRequest = createAction('Add meal request', prop<{ addMealRequest: AddMealItemRequest }>());
export const addMealRequestSuccess = createAction('Add meal request success', prop<{ addedDate: Date }>());
export const addMealRequestFailed = createAction('Add meal request failed', prop<{ errorCode: number }>());

const actions = union({
  getAllMealsRequest,
  getAllMealsRequestSuccess,
  getAllMealsRequestFailed,
  addMealRequest,
  addMealRequestSuccess,
  addMealRequestFailed,
});

export type MealCalendarActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
