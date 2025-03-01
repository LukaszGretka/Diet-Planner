import { DishProduct } from './dish-product';

export class Dish {
  id: number;
  mealDishId: number;
  name: string;
  description: string;
  imagePath: string;
  products: DishProduct[];
  exposeToOtherUsers: boolean;
  isOwner: boolean;
}
