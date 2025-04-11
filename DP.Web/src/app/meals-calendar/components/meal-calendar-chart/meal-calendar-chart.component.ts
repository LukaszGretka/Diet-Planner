import { Component, OnInit, input } from '@angular/core';
import { Meal } from '../../models/meal';
import { ChartData, ChartType } from 'chart.js';
import { MacronutrientsWithCalorties } from '../../models/macronutrients';
import { MealCalendarCalculator } from '../../services/meal-calendar-calculator.service';
import { Observable } from 'rxjs';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NgChartsModule } from 'ng2-charts';
import { DecimalPipe } from '@angular/common';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-chart',
  templateUrl: './meal-calendar-chart.component.html',
  styleUrl: './meal-calendar-chart.component.css',
  imports: [NgChartsModule, DecimalPipe],
})
export class MealCalendarChartComponent implements OnInit {
  public readonly meals = input<Observable<Meal[]>>(undefined);

  public doughnutChartType: ChartType = 'doughnut';
  public doughnutChartData: ChartData<'doughnut'>;
  public doughnutChartLabels: string[] = ['Carbohydrates', 'Proteins', 'Fats'];

  public totalCalories: number;

  public ngOnInit(): void {
    this.meals().pipe(untilDestroyed(this)).subscribe(meals => {
      const dataset = this.buildMacronutrientsChartDataset(meals);
      this.doughnutChartData = {
        labels: this.doughnutChartLabels,
        datasets: [{ data: [dataset.carbs, dataset.proteins, dataset.fats] }],
      };

      this.totalCalories = dataset.calories;
    });
  }

  private buildMacronutrientsChartDataset(meals: Meal[]): MacronutrientsWithCalorties {
    return meals.reduce(
      (totalMacronutrients: MacronutrientsWithCalorties, meal: Meal) => {
        const result: MacronutrientsWithCalorties = MealCalendarCalculator.calculateMealMacronutrients(meal);
        totalMacronutrients.carbs += result.carbs;
        totalMacronutrients.proteins += result.proteins;
        totalMacronutrients.fats += result.fats;
        totalMacronutrients.calories += result.calories;
        return totalMacronutrients;
      },
      { carbs: 0, proteins: 0, fats: 0, calories: 0 } as MacronutrientsWithCalorties,
    );
  }
}
