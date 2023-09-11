import { createAction, union } from '@ngrx/store';
import { Dish } from '../models/dish';

export const updatePortionRequest = createAction(
  'Update portion request trigger',
  prop<{ dishId: number; productId: number; portionMultiplier: number }>(),
);

export const updatePortionRequestSuccess = createAction('Update portion request success');
export const updatePortionRequestFailed = createAction('Update portion request failed');

export const saveDishRequest = createAction('Save dish request', prop<{ dish: Dish; returnUrl: string }>());

export const saveDishRequestSuccess = createAction('Save dish request success');
export const saveDishRequestFailed = createAction('Save dish request failed');

const actions = union({
  updatePortionRequest,
  updatePortionRequestSuccess,
  updatePortionRequestFailed,
  saveDishRequest,
  saveDishRequestSuccess,
  saveDishRequestFailed,
});

export type DishActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
