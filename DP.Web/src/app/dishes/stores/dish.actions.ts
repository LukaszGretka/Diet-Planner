import { createAction, union } from '@ngrx/store';
import { Dish } from '../models/dish';
import { DishProduct } from '../models/dish-product';
import { MealType } from 'src/app/meals-calendar/models/meal-type';

export const loadDishesRequest = createAction('Load dishes request');
export const loadDishesRequestSuccess = createAction('Load dishes request success', prop<{ dishes: Dish[] }>());
export const loadDishesRequestFailed = createAction('Load dishes request failed', prop<{ error: number }>());

export const saveDishRequest = createAction('Save dish request', prop<{ dish: Dish; returnUrl: string }>());
export const saveDishRequestSuccess = createAction('Save dish request success', prop<{ callbackMealDish: any }>());
export const saveDishRequestFailed = createAction('Save dish request failed', prop<{ error: number }>());

export const getDishByNameRequest = createAction('Get dish by name request', prop<{ name: string }>());
export const getDishByNameRequestSuccess = createAction('Get dish by name request success');
export const getDishByNameRequestFailed = createAction('Get dish by name request failed', prop<{ error: number }>());

export const editDishRequest = createAction('Edit dish request', prop<{ dish: Dish; returnUrl: string }>());
export const editDishRequestSuccess = createAction(
  'Edit dish request success',
  prop<{ dishName: string; returnUrl: string }>(),
);
export const editDishRequestFailed = createAction('Edit dish request failed', prop<{ error: number }>());

export const deleteDishRequest = createAction('Delete dish request', prop<{ id: number }>());
export const deleteDishRequestSuccess = createAction('Delete dish request success');
export const deleteDishRequestFailed = createAction('Delete dish request failed', prop<{ error: number }>());

export const getDishProductsRequest = createAction('Get dish products request', prop<{ dishId: number }>());
export const getDishProductsRequestSuccess = createAction(
  'Get dish products request success',
  prop<{ dishProducts: DishProduct[] }>(),
);
export const getDishProductsRequestFailed = createAction('Get dish products request failed', prop<{ error: number }>());

export const setCallbackMealDish = createAction(
  'Set callback meal dish',
  prop<{ dishName: string; mealType: MealType }>(),
);
export const getCallbackMealDish = createAction('Get callback meal dish');
export const clearCallbackMealDish = createAction('Clear callback meal dish');

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
  deleteDishRequest,
  deleteDishRequestSuccess,
  deleteDishRequestFailed,
  getDishProductsRequest,
  getDishProductsRequestSuccess,
  getDishProductsRequestFailed,
  setCallbackMealDish,
  getCallbackMealDish,
  clearCallbackMealDish,
});

export type DishActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
