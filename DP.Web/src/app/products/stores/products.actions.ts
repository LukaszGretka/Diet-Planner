import { createAction, union } from '@ngrx/store';
import { Product } from '../models/product';

export const addProductRequest = createAction(
  'Add product request',
  prop<{ productData: Product; returnUrl?: string }>(),
);
export const addProductRequestCompleted = createAction('Add product request completed');

export const editProductRequest = createAction(
  'Edit product request',
  prop<{ productId: number; productData: Product }>(),
);
export const editProductRequestCompleted = createAction('Edit product request completed');

export const removeProductRequest = createAction('Remove product request', prop<{ productId: number }>());
export const removeProductRequestCompleted = createAction('Remove product request completed');

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

const actions = union({
  addProductRequest,
  addProductRequestCompleted,
  editProductRequest,
  editProductRequestCompleted,
  removeProductRequest,
  removeProductRequestCompleted,
  getAllProductsRequest,
  getAllProductsRequestSuccess,
  getAllProductsRequestFailed,
  getProductByNameRequest,
  getProductByNameRequestSuccess,
  getProductByNameRequestFailed,
});

export type ProductsActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
