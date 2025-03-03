import { DishProduct } from './dish-product';
import { BaseItem } from '../../shared/models/base-item';

export class Dish extends BaseItem {
  mealDishId: number;
  products: DishProduct[];
  exposeToOtherUsers: boolean;
  isOwner: boolean;
  readonly type = "dish";
}
