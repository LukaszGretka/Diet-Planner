import { SpecifiedMeal } from "./specified-meal";

export interface DailyMeals {
  date: Date;
  breakfast: SpecifiedMeal;
  lunch: SpecifiedMeal;
  dinner: SpecifiedMeal;
  supper: SpecifiedMeal;
}
