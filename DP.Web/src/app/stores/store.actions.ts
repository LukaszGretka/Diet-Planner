import { createAction, union } from '@ngrx/store';
import { Measurement } from 'src/app/body-profile/models/measurement';
import { PortionProduct, Product } from 'src/app/products/models/product';

export const setErrorCode = createAction('Set error code', prop<{ errorCode: number | null; errorMessage?: string }>());
export const clearErrors = createAction('Clear errors');

// export const getProductsRequest = createAction('Get products request');
// export const getProductsRequestCompleted = createAction(
//   'Get product request completed',
//   prop<{ products: PortionProduct[] }>(),
// );

export const addProductRequest = createAction('Add product request', prop<{ productData: Product; returnUrl?: string }>());
export const addProductRequestCompleted = createAction('Add product request completed');

export const editProductRequest = createAction(
  'Edit product request',
  prop<{ productId: number; productData: PortionProduct }>(),
);
export const editProductRequestCompleted = createAction('Edit product request completed');

export const removeProductRequest = createAction('Remove product request', prop<{ productId: number }>());
export const removeProductRequestCompleted = createAction('Remove product request completed');

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

export const getDailyMealsRequest = createAction('Get daily meals request');

const actions = union({
  setErrorCode,
  // getProductsRequest,
  addProductRequest,
  addProductRequestCompleted,
  editProductRequest,
  editProductRequestCompleted,
  removeProductRequest,
  removeProductRequestCompleted,
  getMeasurementsRequest,
  getMeasurementsCompleted,
  addMeasurementRequest,
  addMeasurementRequestCompleted,
  editMeasurementRequest,
  editMeasurementRequestCompleted,
  removeMeasurementRequest,
  removeMeasurementRequestCompleted,
  getDailyMealsRequest,
});

export type GeneralActions = typeof actions;

export function prop<T>() {
  return (payload: T) => ({ payload });
}
