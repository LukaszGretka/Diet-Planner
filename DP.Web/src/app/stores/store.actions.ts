import { createAction, union } from '@ngrx/store';

export const setErrorCode = createAction('Set error code', prop<{ errorCode: number | null; errorMessage?: string }>());
export const clearErrors = createAction('Clear errors');

const actions = union({
  setErrorCode,
  clearErrors,
});

export type GeneralActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
