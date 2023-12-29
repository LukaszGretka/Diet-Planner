import { createAction, union } from "@ngrx/store";
import { Measurement } from "../models/measurement";

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


const actions = union({

});

export type BodyProfileActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
