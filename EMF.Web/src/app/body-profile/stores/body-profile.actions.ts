import { createAction, union } from "@ngrx/store";
import { Measurement } from "src/models/measurement";

export const setMeasurement = createAction("Set measurement", 
    prop<{ measurement: Measurement }>());
export const setError = createAction("Set error", prop<{ message: string }>());

export const submitMeasurementRequest = createAction("Submit measurement request");
export const submitMeasurementRequestSuccess = createAction("Submit measurement request success")

const actions = union({
    setMeasurement,
    setError,
    submitMeasurementRequest,
    submitMeasurementRequestSuccess
});

export type BodyProfileAction = typeof actions;

export function prop<T>() {
    return (payload: T) => ({ payload });
}