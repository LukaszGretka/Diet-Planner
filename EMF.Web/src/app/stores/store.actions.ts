import { createAction, union } from "@ngrx/store";
import { Measurement } from "src/models/measurement";
import { Product } from "src/models/product";

export const setMeasurement = createAction("Set measurement", 
    prop<{ measurement: Measurement }>());
export const setError = createAction("Set error", prop<{ message: string }>());

export const submitMeasurementRequest = createAction("Submit measurement request");
export const submitMeasurementRequestSuccess = createAction("Submit measurement request success");

export const setProduct = createAction("Set product", prop<{ product: Product }>());
export const submitAddProductRequest = createAction("Submit add product request");
export const submitAddProductRequestSuccess = createAction("Submit add product request success");

const actions = union({
    setError,
    setMeasurement,
    submitMeasurementRequest,
    submitMeasurementRequestSuccess,
    setProduct,
    submitAddProductRequest,
    submitAddProductRequestSuccess
});

export type GeneralActions = typeof actions;

export function prop<T>() {
    return (payload: T) => ({ payload });
}