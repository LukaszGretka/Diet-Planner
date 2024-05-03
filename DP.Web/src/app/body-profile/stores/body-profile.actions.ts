import { createAction, union } from '@ngrx/store';
import { Measurement } from '../models/measurement';
import { UserProfile } from '../models/user-profile';

export const getMeasurementsRequest = createAction('Get measurement request');
export const getMeasurementsCompleted = createAction(
  'Get measurement completed',
  prop<{ measurements: Measurement[] }>(),
);

export const addMeasurementRequest = createAction('Add measurement request', prop<{ measurementData: Measurement }>());
export const addMeasurementRequestCompleted = createAction('Add measurement request completed');

export const editMeasurementRequest = createAction(
  'Edit measurement request',
  prop<{ measurementId: number; measurementData: Measurement }>(),
);
export const editMeasurementRequestCompleted = createAction('Edit measurement request completed');

export const removeMeasurementRequest = createAction('Remove measurement request', prop<{ measurementId: number }>());
export const removeMeasurementRequestCompleted = createAction('Remove measurement request completed');

export const getUserProfileRequest = createAction('Get user profile request');
export const getUserProfileSuccess = createAction('Get user profile success', prop<{ userProfile: UserProfile }>());
export const getUserProfileFailed = createAction('Get user profile failed', prop<{ error: string }>());

export const updateUserProfileRequest = createAction(
  'Update user profile request',
  prop<{ userProfile: UserProfile }>(),
);
export const updateUserProfileSuccess = createAction('Update user profile success');
export const updateUserProfileFailed = createAction('Update user profile failed', prop<{ error: string }>());

export const uploadUserAvatarRequest = createAction(
  'Upload user avatar request',
  prop<{ base64Avatar: string }>(),
);
export const uploadUserAvatarRequestSuccess = createAction('Upload user avatar request success', prop<{ userProfile: UserProfile }>());
export const uploadUserAvatarRequestFailed = createAction('Upload user avatar request failed', prop<{ error: string }>());

const actions = union({
  getMeasurementsRequest,
  getMeasurementsCompleted,
  addMeasurementRequest,
  addMeasurementRequestCompleted,
  editMeasurementRequest,
  editMeasurementRequestCompleted,
  removeMeasurementRequest,
  removeMeasurementRequestCompleted,
  getUserProfileRequest,
  getUserProfileSuccess,
  getUserProfileFailed,
  updateUserProfileRequest,
  updateUserProfileSuccess,
  updateUserProfileFailed,
  uploadUserAvatarRequest,
  uploadUserAvatarRequestSuccess,
  uploadUserAvatarRequestFailed
});

export type BodyProfileActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
