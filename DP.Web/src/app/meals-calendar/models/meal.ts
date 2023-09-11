import { DishProduct } from 'src/app/dishes/models/dish-product';
import { MealType } from './meal-type';

export interface Meal {
  mealTypeId: MealType;
  portionProducts: DishProduct[];
}

export interface MealByDay extends Meal {
  date: Date;
}
