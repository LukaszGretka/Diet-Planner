import { createFeatureSelector, createSelector } from '@ngrx/store';
import { MealCalendarState } from './meals-calendar.state';
import { MealType } from '../models/meal-type';

const getState = createFeatureSelector<MealCalendarState>('mealCalendarState');

export const getDailyMealsOverview = createSelector(getState, state => state?.dailyMealsOverview);
