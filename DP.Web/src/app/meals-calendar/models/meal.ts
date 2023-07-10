import { PortionProduct } from 'src/app/products/models/product';
import { MealType } from './meal-type';

export interface Meal {
  mealTypeId: MealType;
  portionProducts: PortionProduct[];
}

export interface MealByDay extends Meal {
  date: Date;
}
