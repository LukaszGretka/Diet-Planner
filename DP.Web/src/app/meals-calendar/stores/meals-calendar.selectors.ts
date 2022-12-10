import { createFeatureSelector, createSelector } from "@ngrx/store";
import { MealCalendarState } from "./meals-calendar.state";

const getState = createFeatureSelector<MealCalendarState>('mealCalendarState');

export const getDailyMealsOverview = createSelector(
  getState,
  state => state?.dailyMealsOverview
);

export const getError = createSelector(
  getState,
  state => state.error
);
