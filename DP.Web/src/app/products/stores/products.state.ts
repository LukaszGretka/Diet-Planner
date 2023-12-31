import { Product } from '../models/product';

export interface ProductsState {
  products: Product[];
  isLoading: boolean;
}
