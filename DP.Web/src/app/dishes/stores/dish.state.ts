import { Dish } from '../models/dish';

export interface DishState {
  dishes: Dish[];
  isLoading: boolean;
}
