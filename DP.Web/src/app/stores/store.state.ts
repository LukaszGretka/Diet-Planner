import { Measurement } from "src/models/measurement";
import { Product } from "src/models/product";

export interface GeneralState {
    measurementData: Measurement;
    error: string;
    productData: Product;
    processingProductId: string;
}