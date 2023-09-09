import { DishProduct } from './dish-product';

export interface Dish {
  name: string;
  description: string;
  imagePath: string;
  products: DishProduct[];
}
