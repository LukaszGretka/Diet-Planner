import { createAction, union } from "@ngrx/store";
import { LogInRequest } from "../models/log-in-request";
import { SignUpRequest } from "../models/sign-up-request";
import { User } from "../models/user";


export const logInRequest = createAction("Log-in request",
  prop<{ logInRequest: LogInRequest }>());
export const logInRequestSuccess = createAction("Log-in request success", prop<{ user: User }>());
export const logInRequestFailed = createAction("Log-in request failed", prop<{ error: string }>());

export const signUpRequest = createAction("Sign-up request",
  prop<{ signUpRequest: SignUpRequest }>());
export const signUpSuccess = createAction("Sign-up request success", prop<{ user: User }>());
export const signUpRequestFailed = createAction("Sign-up request failed", prop<{ error: string }>());

const actions = union({
  logInRequest,
  logInRequestSuccess,
  logInRequestFailed,
  signUpRequest,
  signUpSuccess,
  signUpRequestFailed
});

export type AccountActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
