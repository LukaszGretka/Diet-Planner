import { MealType } from './meal-type';

export class MealCalendarConfig {
  breakfast: MealConfig;
  lunch: MealConfig;
  dinner: MealConfig;
  supper: MealConfig;
}

export class MealConfig {
  name: string;
  type: MealType;
  isVisible: boolean;
}
