import { createReducer, on } from '@ngrx/store';
import * as accountActions from './account.actions';
import { AccountState } from './account.state';

export const initialState: AccountState = {
  isLoading: false,
};

const reducerFactory = createReducer(
  initialState,
  on(accountActions.signInRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(accountActions.signInRequestSuccess, state => ({
    ...state,
    isLoading: false,
  })),
  on(accountActions.signInRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
  on(accountActions.signUpRequest, state => ({
    ...state,
    isLoading: true,
  })),
  on(accountActions.signUpRequestSuccess, state => ({
    ...state,
    isLoading: false,
  })),
  on(accountActions.signUpRequestFailed, state => ({
    ...state,
    isLoading: false,
  })),
);

export function AccountReducer(
  state: AccountState = initialState,
  action: accountActions.AccountActions,
): AccountState {
  return reducerFactory(state, action);
}
