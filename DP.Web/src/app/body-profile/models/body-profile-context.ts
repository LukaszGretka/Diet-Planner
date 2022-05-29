import { Measurement } from "src/app/body-profile/models/measurement";

export interface BodyProfileContext {
    measurementData: Measurement;
    measurementError: string;
}
