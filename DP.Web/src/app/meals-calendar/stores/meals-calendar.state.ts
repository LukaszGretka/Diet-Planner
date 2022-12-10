import { DailyMealsOverview } from "../models/daily-meals-overview";
import { Meal } from "../models/meal";

export interface MealCalendarState {
  dailyMealsOverview: Meal[];
  error: string;
}
