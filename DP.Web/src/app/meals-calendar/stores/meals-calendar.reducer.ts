import { createReducer, on } from '@ngrx/store';
import { MealCalendarState } from './meals-calendar.state';
import * as mealCalendarActions from './meals-calendar.actions';

export const initialState: MealCalendarState = {
  allDailyMeals: [],
  errorCode: null,
};

const reducerFactory = createReducer(
  initialState,
  on(mealCalendarActions.getAllMealsRequestSuccess, (state, action) => ({
    ...state,
    allDailyMeals: action.payload.result,
  })),
  on(mealCalendarActions.getAllMealsRequestFailed, (state, action) => ({
    ...state,
    errorCode: action.payload.errorCode == 0 ? 503 : action.payload.errorCode,
  })),
  on(mealCalendarActions.addMealRequestSuccess, (state, action) => ({
    ...state,
    allDailyMeals: action.payload.result,
  })),
  on(mealCalendarActions.removeMealItemSuccess, (state, action) => ({
    ...state,
    allDailyMeals: action.payload.result,
  })),
  on(mealCalendarActions.updateMealItemPortionSuccess, (state, action) => ({
    ...state,
    allDailyMeals: action.payload.result,
  })),
);

export function MealCalendarReducer(
  state: MealCalendarState = initialState,
  action: mealCalendarActions.MealCalendarActions,
): MealCalendarState {
  return reducerFactory(state, action);
}
