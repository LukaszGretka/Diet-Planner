import { createFeatureSelector, createSelector } from "@ngrx/store";
import { AccountState } from "./account.state";

const getState = createFeatureSelector<AccountState>('accountState');

export const getAuthenticatedUser = createSelector(
  getState,
  state => state.authenticatedUser
);
