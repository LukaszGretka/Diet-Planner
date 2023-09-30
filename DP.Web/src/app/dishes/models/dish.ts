import { DishProduct } from './dish-product';

export class Dish {
  id: number;
  name: string;
  description: string;
  imagePath: string;
  products: DishProduct[];
  exposeToOtherUsers: boolean;
  totalMacro: any;
}
