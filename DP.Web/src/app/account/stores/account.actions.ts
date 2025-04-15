import { createAction, union } from '@ngrx/store';
import { SignInRequest } from '../models/sign-in-request';
import { SignUpRequest } from '../models/sign-up-request';
import { User } from '../models/user';
import { SignInResult } from '../models/sign-in-result';
import { SignUpResult } from '../models/sign-up-result';
import { EmailConfirmationRequest } from '../models/email-confirmation-request';
import { ChangePasswordRequest } from '../models/change-password-request';

export const getUserRequest = createAction('Get user request');
export const getUserRequestSuccess = createAction('Get user request success', prop<{ user: User }>());
export const getUserRequestFailed = createAction('Get user request failed', prop<{ error: string }>());

export const signInRequest = createAction('Sign-in request', prop<{ signInRequest: SignInRequest }>());
export const signInRequestSuccess = createAction('Sign-in request success', prop<{ signInResult: SignInResult }>());
export const signInRequestFailed = createAction('Sign-in request failed', prop<{ error: string }>());

export const signUpRequest = createAction('Sign-up request', prop<{ signUpRequest: SignUpRequest }>());
export const signUpRequestSuccess = createAction('Sign-up request success', prop<{ signUpResult: SignUpResult }>());
export const signUpRequestFailed = createAction('Sign-up request failed', prop<{ error: string }>());

export const signOutRequest = createAction('Sign out request');
export const signOutRequestSuccess = createAction('Sign out request success');
export const signOutRequestFailed = createAction('Sign out request failed', prop<{ error: string }>());

export const confirmEmailRequest = createAction(
  'Confirm email request',
  prop<{ emailConfirmationRequest: EmailConfirmationRequest }>(),
);
export const confirmEmailRequestSuccess = createAction('Confirm email request success');
export const confirmEmailRequestFailed = createAction('Confirm email request failed', prop<{ error: string }>());

export const changePasswordRequest = createAction(
  'Change password request',
  prop<{ changePasswordRequest: ChangePasswordRequest }>(),
);

export const changePasswordRequestSuccess = createAction('Change password request success');
export const changePasswordRequestFailed = createAction('Change password request failed', prop<{ error: string }>());

const actions = union({
  getUserRequest,
  getUserRequestSuccess,
  getUserRequestFailed,
  signInRequest,
  signInRequestSuccess,
  signInRequestFailed,
  signUpRequest,
  signUpRequestSuccess,
  signUpRequestFailed,
  signOutRequest,
  signOutRequestSuccess,
  signOutRequestFailed,
  confirmEmailRequest,
  confirmEmailRequestSuccess,
  confirmEmailRequestFailed,
  changePasswordRequest,
  changePasswordRequestSuccess,
  changePasswordRequestFailed,
});

export type AccountActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
