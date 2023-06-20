import { MealType } from 'src/app/meals-calendar/models/meal-type';

export interface ProductsState {
  callbackMealProduct: { productName: string; mealType: MealType };
}
