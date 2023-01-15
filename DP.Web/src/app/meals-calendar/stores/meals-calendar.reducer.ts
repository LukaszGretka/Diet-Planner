import { createReducer, on } from "@ngrx/store";
import { MealCalendarState } from "./meals-calendar.state";
import * as mealCalendarActions from "./meals-calendar.actions";

export const initialState: MealCalendarState = {
  dailyMealsOverview: [],
  error: '',
};

const reducerFactory = createReducer(
  initialState,
  on(mealCalendarActions.getMealsRequestSuccess,
    (state, action) => ({
      ...state,
      dailyMealsOverview: action.payload.result,
    })),
  on(mealCalendarActions.getMealsRequestFailed,
    (state, action) => ({
      ...state,
      error: action.payload.error
    }))
)

export function MealCalendarReducer(
  state: MealCalendarState = initialState,
  action: mealCalendarActions.MealCalendarActions): MealCalendarState {
  return reducerFactory(state, action);
}
