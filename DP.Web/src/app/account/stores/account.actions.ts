import { createAction, union } from "@ngrx/store";
import { LogInRequest } from "../models/log-in-request";
import { SignUpRequest } from "../models/sign-up-request";
import { User } from "../models/user";
import { LogInResult } from "../models/log-in-result";


export const logInRequest = createAction("Log-in request",
  prop<{ logInRequest: LogInRequest }>());
export const logInRequestSuccess = createAction("Log-in request success", prop<{ logInResult: LogInResult }>());
export const logInRequestFailed = createAction("Log-in request failed", prop<{ error: string }>());

export const signUpRequest = createAction("Sign-up request",
  prop<{ signUpRequest: SignUpRequest }>());
export const signUpSuccess = createAction("Sign-up request success", prop<{ user: User }>());
export const signUpRequestFailed = createAction("Sign-up request failed", prop<{ error: string }>());

export const signOutRequest = createAction("Sign out request");
export const signOutRequestSuccess = createAction("Sign out request success");
export const signOutRequestFailed = createAction("Sign out request failed", prop<{ error: string }>());

const actions = union({
  logInRequest,
  logInRequestSuccess,
  logInRequestFailed,
  signUpRequest,
  signUpSuccess,
  signUpRequestFailed,
  signOutRequest,
  signOutRequestSuccess,
  signOutRequestFailed
});

export type AccountActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
