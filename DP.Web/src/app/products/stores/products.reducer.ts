import { createReducer, on } from '@ngrx/store';
import { ProductsState } from './products.state';
import * as productsActions from './products.actions';

export const initialState: ProductsState = {
  products: [],
  callbackMealProduct: null,
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
  on(productsActions.setCallbackMealProduct, (state, action) => ({
    ...state,
    callbackMealProduct: { productName: action.payload?.productName, mealType: action.payload?.mealType },
  })),
  on(productsActions.clearCallbackMealProduct, state => ({
    ...state,
    callbackMealProduct: null,
  })),
);

export function ProductsReducer(
  state: ProductsState = initialState,
  action: productsActions.ProductsActions,
): ProductsState {
  return reducerFactory(state, action);
}
