import { createAction, union } from "@ngrx/store";
import { Measurement } from "src/models/measurement";
import { Product } from "src/models/product";

export const clearState = createAction("Clear state");

export const setError = createAction("Set error", prop<{ message: string }>());
export const setMeasurement = createAction("Set measurement",
  prop<{ measurement: Measurement }>());

export const submitMeasurementRequest = createAction("Submit measurement request");
export const submitMeasurementRequestSuccess = createAction("Submit measurement request success");

export const setProduct = createAction("Set product", prop<{ product: Product }>());
export const submitAddProductRequest = createAction("Submit add product request");
export const submitAddProductRequestSuccess = createAction("Submit add product request success");

export const setProcessingProductId = createAction("Set processing product id", prop<{ id: string }>());
export const submitRemoveProductRequest = createAction("Submit remove product request");
export const submitRemoveProductRequestSuccess = createAction("Submit remove product request");

export const submitEditProductRequest = createAction("Submit edit product request", prop<{ product: Product }>());
export const submitEditProductRequestSuccess = createAction("Submit edit product request success");

export const submitRemoveMeasurementRequest = createAction("Submit remove measurement request", prop<{ id: string }>());

export const submitEditMeasurementRequest = createAction("Submit edit measurement request",
  prop<{ measurement: Measurement }>());

export const submitEditMeasurementRequestSuccess = createAction("Submit edit measurement request success");

const actions = union({
  clearState,
  setError,
  setMeasurement,
  submitMeasurementRequest,
  submitMeasurementRequestSuccess,
  setProduct,
  setProcessingProductId,
  submitAddProductRequest,
  submitAddProductRequestSuccess,
  submitRemoveProductRequest,
  submitRemoveProductRequestSuccess,
  submitEditProductRequest,
  submitEditProductRequestSuccess,
  submitRemoveMeasurementRequest,
  submitEditMeasurementRequest,
  submitEditMeasurementRequestSuccess
});

export type GeneralActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
