import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProductsState } from './products.state';

const getState = createFeatureSelector<ProductsState>('productsState');

export const getAllProducts = createSelector(getState, state => state.products);
