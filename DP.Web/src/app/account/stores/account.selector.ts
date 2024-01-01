import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AccountState } from './account.state';

const getState = createFeatureSelector<AccountState>('accountState');

export const isLoading = createSelector(getState, state => state.isLoading);
