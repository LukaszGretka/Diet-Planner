import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { Dish } from '../models/dish';

export interface DishState {
  dishes: Dish[];
  callbackMealDish: { dishName: string; mealType: MealType };
  isLoading: boolean;
}
