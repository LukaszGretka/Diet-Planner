import { Component, OnInit, inject, input } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { MacronutrientsWithCalorties } from '../../../models/macronutrients';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';
import { MealCalendarState } from 'src/app/meals-calendar/stores/meals-calendar.state';
import * as MealCalendarSelectors from 'src/app/meals-calendar/stores/meals-calendar.selectors';
import { Store } from '@ngrx/store';
import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: '[app-meal-summary-row]',
  imports: [DecimalPipe],
  templateUrl: './meal-summary-row.component.html',
  styleUrl: './meal-summary-row.component.css',
})
export class MealSummaryRowComponent implements OnInit {
  private readonly mealCalendarStore = inject<Store<MealCalendarState>>(Store);

  public readonly mealType = input<MealType>(undefined);
  public mealMacroSummary: MacronutrientsWithCalorties;

  public ngOnInit(): void {
    this.mealCalendarStore
      .select(MealCalendarSelectors.getMealByMealType(this.mealType()))
      .pipe(untilDestroyed(this))
      .subscribe(meal => {
        if (meal) {
          this.mealMacroSummary = MealCalendarCalculator.calculateMealMacronutrients(meal);
        }
      });
  }
}
