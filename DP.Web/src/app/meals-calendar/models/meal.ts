import { DishProduct } from 'src/app/dishes/models/dish-product';
import { MealType } from './meal-type';
import { Dish } from 'src/app/dishes/models/dish';

export interface Meal {
  mealTypeId: MealType;
  dishes: Dish[];
}

export interface MealByDay extends Meal {
  date: Date;
}
