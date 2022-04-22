import { createReducer, on } from "@ngrx/store";
import * as bodyProfileActions from "./body-profile.actions";
import { BodyProfileState } from "./body-profile.state";

export const initialState: BodyProfileState = {
    measurementData : null,
    addMeasurementError: ''
};

const reducerFactory = createReducer(
    initialState,
    on(bodyProfileActions.setMeasurement,
        (state, action) => ({
            ...state,
            measurementData: action.payload.measurement
        })),
    on(bodyProfileActions.setError,
        (state, action) => ({
            ...state,
            addMeasurementError: action.payload.message
        })),
);

export function BodyProfileReducer(
    state: BodyProfileState = initialState,
    action: bodyProfileActions.BodyProfileAction): BodyProfileState {
    return reducerFactory(state, action);
}