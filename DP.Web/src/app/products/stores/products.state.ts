import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { Product } from '../models/product';

export interface ProductsState {
  products: Product[];
  callbackMealProduct: { productName: string; mealType: MealType };
  isLoading: boolean;
}
