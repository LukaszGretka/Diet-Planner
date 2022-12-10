import { Product } from 'src/app/products/models/product';
import { MealType } from './meal-type';

export interface Meal {
  mealTypeId: MealType;
  products: Product[];
}

export interface MealByDay extends Meal {
  date: Date;
}
