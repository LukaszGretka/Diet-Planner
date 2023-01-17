import {Measurement} from '../body-profile/models/measurement';
import {Product} from '../products/models/product';

export interface GeneralState {
  errorCode: number | null;
  measurements: Measurement[];
  products: Product[];
}
