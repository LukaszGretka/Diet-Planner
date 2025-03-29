import { DishProduct } from 'src/app/dishes/models/dish-product';
import { MealType } from './meal-type';
import { Dish } from 'src/app/dishes/models/dish';
import { Product } from '../../products/models/product';
import { ItemType } from '../../shared/models/base-item';

export interface Meal {
  date: Date;
  mealType: MealType;
  dishes?: Dish[];
  products?: Product[];
}

export interface AddMealItemRequest {
  date: Date;
  mealType: MealType;
  itemId: number;
  itemType: ItemType;
}

