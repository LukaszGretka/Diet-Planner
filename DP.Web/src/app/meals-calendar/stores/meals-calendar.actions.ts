import { createAction, union } from '@ngrx/store';
import { MealByDay } from '../models/meal';

export const addMealRequest = createAction('Add meal by day', prop<{ mealByDay: MealByDay }>());
export const addMealRequestCompleted = createAction('Add meal by day completed');

const actions = union({
	addMealRequest,
	addMealRequestCompleted,
});

export type MealCalendarActions = typeof actions;

export function prop<T>() {
	return (payload: T) => ({ payload });
}
