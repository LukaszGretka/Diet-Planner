import { Measurement } from "src/models/measurement";

export interface BodyProfileContext {
    measurementData: Measurement;
    measurementError: string;
}