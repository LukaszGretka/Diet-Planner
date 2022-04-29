import { createReducer, on } from "@ngrx/store";
import * as generalActions from "./store.actions";
import { GeneralState } from "./store.state";

export const initialState: GeneralState = {
    measurementData: null,
    addMeasurementError: '',
    productData: null,
    processingProductId: ''
};

const reducerFactory = createReducer(
    initialState,
    on(generalActions.clearState,
        (state) => ({
            ...state,
            measurementData: null,
            addMeasurementError: '',
            productData: null,
            processingProductId: ''
        })),
    on(generalActions.setMeasurement,
        (state, action) => ({
            ...state,
            measurementData: action.payload.measurement
        })),
    on(generalActions.setError,
        (state, action) => ({
            ...state,
            addMeasurementError: action.payload.message
        })),
    on(generalActions.setProduct,
        (state, action) => ({
            ...state,
            productData: action.payload.product
        })),
    on(generalActions.setProcessingProductId,
        (state, action) => ({
            ...state,
            processingProductId: action.payload.id
        })),
);

export function GeneralReducer(
    state: GeneralState = initialState,
    action: generalActions.GeneralActions): GeneralState {
    return reducerFactory(state, action);
}