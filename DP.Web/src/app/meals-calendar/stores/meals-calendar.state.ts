import { Meal } from '../models/meal';

export interface MealCalendarState {
  allDailyMeals: Meal[];
  errorCode: number | null;
}
