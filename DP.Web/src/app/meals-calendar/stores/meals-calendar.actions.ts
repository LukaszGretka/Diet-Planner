import { createAction, union } from '@ngrx/store';
import { Meal, MealItemRequest, UpdateMealItemPortionRequest } from '../models/meal';

export const getAllMealsRequest = createAction('Get meals request', prop<{ date: Date }>());
export const getAllMealsRequestSuccess = createAction('Get all meals request success', prop<{ result: Meal[] }>());
export const getAllMealsRequestFailed = createAction('Get all meals request failed', prop<{ errorCode: number }>());

export const addMealRequest = createAction('Add meal request', prop<{ addMealRequest: MealItemRequest }>());
export const addMealRequestSuccess = createAction('Add meal success', prop<{ addedDate: Date }>());
export const addMealRequestFailed = createAction('Add meal failed', prop<{ errorCode: number }>());

export const removeMealItemRequest = createAction(
  'Remove meal item request',
  prop<{ removeMealRequest: MealItemRequest }>(),
);
export const removeMealItemSuccess = createAction('Remove meal item success', prop<{ addedDate: Date }>());
export const removeMealItemFailed = createAction('Remove meal item failed', prop<{ errorCode: number }>());

export const updateMealItemPortionRequest = createAction(
  'Update meal item portion request',
  prop<{ request: UpdateMealItemPortionRequest }>(),
);

export const updateMealItemPortionSuccess = createAction('Update meal item portion success');
export const updateMealItemPortionFailed = createAction('Update meal item portion failed', prop<{ error: number }>());

const actions = union({
  getAllMealsRequest,
  getAllMealsRequestSuccess,
  getAllMealsRequestFailed,
  addMealRequest,
  addMealRequestSuccess,
  addMealRequestFailed,
  removeMealItemRequest,
  removeMealItemSuccess,
  removeMealItemFailed,
  updateMealItemPortionRequest,
  updateMealItemPortionSuccess,
  updateMealItemPortionFailed,
});

export type MealCalendarActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
