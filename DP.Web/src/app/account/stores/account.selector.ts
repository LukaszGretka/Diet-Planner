import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AccountState } from "./account.state";

const getState = createFeatureSelector<AccountState>('accountState');

export const getUser = createSelector(
  getState,
  state => state.user 
);
