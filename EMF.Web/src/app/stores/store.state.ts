import { Measurement } from "src/models/measurement";
import { Product } from "src/models/product";

export interface GeneralState {
    measurementData: Measurement;
    addMeasurementError: string;

    productData: Product;
}