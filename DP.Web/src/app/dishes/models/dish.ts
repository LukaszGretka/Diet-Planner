import { DishProduct } from './dish-product';

export interface Dish {
  id: number;
  name: string;
  description: string;
  imagePath: string;
  products: DishProduct[];
  exposeToOtherUsers: boolean;
}
