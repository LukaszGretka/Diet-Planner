import { Injectable } from '@angular/core';
import { Meal } from '../models/meal';
import { MacronutrientsWithCalorties } from '../models/macronutrients';
import { Dish } from 'src/app/dishes/models/dish';
import { Product } from 'src/app/products/models/product';

@Injectable({
  providedIn: 'root',
})
export class MealCalendarCalculator {
  // Calculates total calories for meals.
  public static calculateTotalCalories(meals: Meal[]): number {
    return meals.reduce((totalSum, meal) => {
      const dishCalories = meal.dishes.reduce(
        (dishSum, dish) =>
          dishSum +
          dish.products.reduce(
            (productSum, dishProduct) =>
              productSum +
              dishProduct.product.calories * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier),
            0,
          ),
        0,
      );

      const productCalories = meal.products.reduce((productSum, product) => productSum + product.calories, 0);

      return totalSum + dishCalories + productCalories;
    }, 0);
  }

  public static calculateMealMacronutrients(meal: Meal): MacronutrientsWithCalorties {
    return meal.dishes?.reduce(
      (macronutrients, dish) => {
        const dishMacros = this.calculateDishProductsMacros(dish);
        macronutrients.carbs += dishMacros.carbs;
        macronutrients.proteins += dishMacros.proteins;
        macronutrients.fats += dishMacros.fats;
        macronutrients.calories += dishMacros.calories;
        return macronutrients;
      },
      meal.products?.reduce(
        (macronutrients, product) => {
          const productMacros = this.calculateProductMacros(product);
          macronutrients.carbs += productMacros.carbs;
          macronutrients.proteins += productMacros.proteins;
          macronutrients.fats += productMacros.fats;
          macronutrients.calories += productMacros.calories;
          return macronutrients;
        },
        { carbs: 0, proteins: 0, fats: 0, calories: 0 } as MacronutrientsWithCalorties,
      ) || { carbs: 0, proteins: 0, fats: 0, calories: 0 },
    );
  }

  public static calculateDishProductsMacros(dish: Dish): MacronutrientsWithCalorties {
    let dishMacro = { carbs: 0, proteins: 0, fats: 0, calories: 0 };
    dish.products.forEach(dishProduct => {
      dishMacro.carbs +=
        dishProduct.product.carbohydrates * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.proteins +=
        dishProduct.product.proteins * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.fats +=
        dishProduct.product.fats * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
      dishMacro.calories +=
        dishProduct.product.calories * (dishProduct.customizedPortionMultiplier ?? dishProduct.portionMultiplier);
    });

    return dishMacro;
  }

  public static calculateProductMacros(product: Product): MacronutrientsWithCalorties {
    let totalMacros = { carbs: 0, proteins: 0, fats: 0, calories: 0 } as MacronutrientsWithCalorties;
    totalMacros.carbs += product.carbohydrates;

    totalMacros.proteins += product.proteins;
    totalMacros.fats += product.fats;
    totalMacros.calories += product.calories;

    return totalMacros;
  }
}
