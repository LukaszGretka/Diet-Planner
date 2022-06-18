import { Meal } from './meal';

export interface DailyMealsOverview {
  date: Date;
  breakfast: Meal;
  lunch: Meal;
  dinner: Meal;
  supper: Meal;
}
