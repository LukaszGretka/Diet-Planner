import { createReducer, on } from "@ngrx/store";
import * as accountActions from "./account.actions";
import { AccountState } from "./account.state";

export const initialState: AccountState = {
  user: null
};

const reducerFactory = createReducer(
  initialState,
  on(accountActions.logInRequestSuccess,
    (state, action) => ({
      ...state,
      user: action.payload.user,
    })),
  on(accountActions.signUpSuccess,
    (state, action) => ({
      ...state,
      user: action.payload.user,
    })),
)

export function AccountReducer(
  state: AccountState = initialState,
  action: accountActions.AccountActions): AccountState {
  return reducerFactory(state, action);
}
