import { createAction, union } from '@ngrx/store';
import { Product } from '../models/product';
import { MealType } from 'src/app/meals-calendar/models/meal-type';

export const getAllProductsRequest = createAction('Get all products request');

export const getAllProductsRequestSuccess = createAction(
  'Get all products request success',
  prop<{ result: Product[] }>(),
);

export const getAllProductsRequestFailed = createAction(
  'Get all products request failed',
  prop<{ errorCode: number }>(),
);

export const getProductByNameRequest = createAction('Get product by name request', prop<{ name: string }>());
export const getProductByNameRequestSuccess = createAction(
  'Get product by name request success',
  prop<{ result: Product }>(),
);
export const getProductByNameRequestFailed = createAction(
  'Get product by name request failed',
  prop<{ errorCode: number }>(),
);

export const setCallbackMealProduct = createAction(
  'Set callback meal product',
  prop<{ productName: string; mealType: MealType }>(),
);

export const getCallbackMealProduct = createAction('Get callback meal product');
export const clearCallbackMealProduct = createAction('Clear callback meal product');

const actions = union({
  getAllProductsRequest,
  getAllProductsRequestSuccess,
  getAllProductsRequestFailed,
  getProductByNameRequest,
  getProductByNameRequestSuccess,
  getProductByNameRequestFailed,
  setCallbackMealProduct,
  getCallbackMealProduct,
  clearCallbackMealProduct,
});

export type ProductsActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
