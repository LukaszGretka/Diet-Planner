import { Component, OnInit, input } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MacronutrientsWithCalorties } from '../../../models/macronutrients';
import { Meal } from 'src/app/meals-calendar/models/meal';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';

@Component({
    selector: '[app-meal-summary-row]',
    imports: [DecimalPipe],
    templateUrl: './meal-summary-row.component.html',
    styleUrl: './meal-summary-row.component.css'
})
export class MealSummaryRowComponent implements OnInit {
  public readonly meal = input<Meal>(undefined);
  public mealMacroSummary: MacronutrientsWithCalorties;

  public ngOnInit(): void {
    this.mealMacroSummary = MealCalendarCalculator.calculateMealMacronutrients(this.meal());
  }
}
