import { Product } from 'src/app/products/models/product';

export class DishProduct {
  dishProductId: number;
  product: Product;
  portionMultiplier: number = 1;
  customizedPortionMultiplier: number = 1;
}
