import {createAction, union} from '@ngrx/store';
import {SignInRequest} from '../models/sign-in-request';
import {SignUpRequest} from '../models/sign-up-request';
import {User} from '../models/user';
import {SignInResult} from '../models/sign-in-result';

export const signInRequest = createAction('Sign-in request', prop<{signInRequest: SignInRequest}>());
export const signInRequestSuccess = createAction('Sign-in request success', prop<{signInResult: SignInResult}>());
export const signInRequestFailed = createAction('Sign-in request failed', prop<{error: string}>());

export const signUpRequest = createAction('Sign-up request', prop<{signUpRequest: SignUpRequest}>());
export const signUpSuccess = createAction('Sign-up request success', prop<{user: User}>());
export const signUpRequestFailed = createAction('Sign-up request failed', prop<{error: string}>());

export const signOutRequest = createAction('Sign out request');
export const signOutRequestSuccess = createAction('Sign out request success');
export const signOutRequestFailed = createAction('Sign out request failed', prop<{error: string}>());

const actions = union({
  signInRequest,
  signInRequestSuccess,
  signInRequestFailed,
  signUpRequest,
  signUpSuccess,
  signUpRequestFailed,
  signOutRequest,
  signOutRequestSuccess,
  signOutRequestFailed,
});

export type AccountActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({payload});
}
