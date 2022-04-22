import { Measurement } from "src/models/measurement";

export interface BodyProfileState {
    measurementData: Measurement;
    addMeasurementError: string;
}