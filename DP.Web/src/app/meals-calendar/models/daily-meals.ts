import { Meal } from './meal';

export interface DailyMeals {
  meals: Record<string, Meal | undefined>;
}
