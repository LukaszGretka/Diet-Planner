import { createReducer, on } from '@ngrx/store';
import { ProductsState } from './products.state';
import * as productsActions from './products.actions';

export const initialState: ProductsState = {
  products: [],
  isLoading: false,
};

const reducerFactory = createReducer(
  initialState,
  on(productsActions.getAllProductsRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(productsActions.getAllProductsRequestSuccess, (state, action) => ({
    ...state,
    products: action.payload.result,
    isLoading: false,
  })),
  on(productsActions.getAllProductsRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
);

export function ProductsReducer(
  state: ProductsState = initialState,
  action: productsActions.ProductsActions,
): ProductsState {
  return reducerFactory(state, action);
}
