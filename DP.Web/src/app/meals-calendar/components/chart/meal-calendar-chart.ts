import { ChartData, ChartType } from 'chart.js';
import { Meal } from '../../models/meal';
import { MealCalendarCalculator } from '../../services/meal-calendar-calculator.service';
import { Macronutrients } from '../../models/macronutrients';

export class MealCalendarChart {
  public doughnutChartLabels: string[] = ['Carbohydrates', 'Proteins', 'Fats'];
  public doughnutChartData: ChartData<'doughnut'> = {
    labels: this.doughnutChartLabels,
    datasets: [{ data: [0, 0, 0] }],
  };

  public doughnutChartType: ChartType = 'doughnut';

  public buildDoughnutChartData(meals: Meal[]): void {
    const dataset = this.buildMacronutrientsChartDataset(meals);
    this.doughnutChartData = {
      labels: this.doughnutChartLabels,
      datasets: [{ data: [dataset.carbs, dataset.proteins, dataset.fats] }],
    };
  }

  private buildMacronutrientsChartDataset(meals: Meal[]): Macronutrients {
    let totalMacronutrients: Macronutrients = { carbs: 0, proteins: 0, fats: 0 };

    meals.forEach((meal: Meal) => {
      let result: Macronutrients = MealCalendarCalculator.calculateMealMacronutrients(meal);
      totalMacronutrients.carbs += result.carbs;
      totalMacronutrients.proteins += result.proteins;
      totalMacronutrients.fats += result.fats;
    });

    return {
      carbs: totalMacronutrients.carbs,
      proteins: totalMacronutrients.proteins,
      fats: totalMacronutrients.fats,
    };
  }
}
