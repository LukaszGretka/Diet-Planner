import { createAction, union } from '@ngrx/store';
import { Dish } from '../models/dish';
import { Product } from 'src/app/products/models/product';
import { DishProduct } from '../models/dish-product';

export const loadDishesRequest = createAction('Load dishes request');
export const loadDishesRequestSuccess = createAction('Load dishes request success', prop<{ dishes: Dish[] }>());
export const loadDishesRequestFailed = createAction('Load dishes request failed', prop<{ error: number }>());

export const saveDishRequest = createAction('Save dish request', prop<{ dish: Dish; returnUrl: string }>());
export const saveDishRequestSuccess = createAction('Save dish request success');
export const saveDishRequestFailed = createAction('Save dish request failed', prop<{ error: number }>());

export const editDishRequest = createAction('Edit dish request', prop<{ dish: Dish }>());
export const editDishRequestSuccess = createAction('Edit dish request success', prop<{ dishName: string }>());
export const editDishRequestFailed = createAction('Edit dish request failed', prop<{ error: number }>());

export const updatePortionRequest = createAction(
  'Update portion request trigger',
  prop<{ dishId: number; productId: number; portionMultiplier: number }>(),
);

export const getDishProductsRequest = createAction('Get dish products request', prop<{ dishId: number }>());
export const getDishProductsRequestSuccess = createAction(
  'Get dish products request success',
  prop<{ dishProducts: DishProduct[] }>(),
);
export const getDishProductsRequestFailed = createAction('Get dish products request failed', prop<{ error: number }>());

export const updatePortionRequestSuccess = createAction('Update portion request success');
export const updatePortionRequestFailed = createAction('Update portion request failed', prop<{ error: number }>());

const actions = union({
  loadDishesRequest,
  loadDishesRequestSuccess,
  loadDishesRequestFailed,
  saveDishRequest,
  saveDishRequestSuccess,
  saveDishRequestFailed,
  editDishRequest,
  editDishRequestSuccess,
  editDishRequestFailed,
  getDishProductsRequest,
  getDishProductsRequestSuccess,
  getDishProductsRequestFailed,
  updatePortionRequest,
  updatePortionRequestSuccess,
  updatePortionRequestFailed,
});

export type DishActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
