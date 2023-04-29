import {createReducer} from '@ngrx/store';
import * as accountActions from './account.actions';
import {AccountState} from './account.state';

export const initialState: AccountState = {};

const reducerFactory = createReducer(initialState);

export function AccountReducer(
  state: AccountState = initialState,
  action: accountActions.AccountActions,
): AccountState {
  return reducerFactory(state, action);
}
