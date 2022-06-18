import { Product } from 'src/app/products/models/product';
import { MealType } from './meal-type';

export interface Meal {
  mealType: MealType;
  products: Product[];
}
