export class Macronutrients {
  carbs: number;
  proteins: number;
  fats: number;
}

export class MacronutrientsWithCalorties extends Macronutrients {
  calories: number;
}

export class MealDishRowDetails extends MacronutrientsWithCalorties {
  portion: number;
}
