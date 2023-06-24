import { createReducer, on } from '@ngrx/store';
import { ProductsState } from './products.state';
import * as productsActions from './products.actions';

export const initialState: ProductsState = {
  callbackMealProduct: null,
};

const reducerFactory = createReducer(
  initialState,
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
