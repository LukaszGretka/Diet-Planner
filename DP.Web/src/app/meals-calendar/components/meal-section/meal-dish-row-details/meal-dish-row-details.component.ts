import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, inject, OnInit, input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Dish } from 'src/app/dishes/models/dish';
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { MealCalendarState } from 'src/app/meals-calendar/stores/meals-calendar.state';
import * as MealCalendarActions from '../../../stores/meals-calendar.actions';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';
import { ItemType } from 'src/app/shared/models/base-item';

@Component({
  selector: '[app-meal-dish-row-details]',
  templateUrl: 'meal-dish-row-details.component.html',
  imports: [DecimalPipe, CommonModule, FormsModule],
})
export class MealDishRowDetailsComponent implements OnInit {
  public readonly dish = input<Dish>(undefined);
  public readonly selectedDate = input<Date>(undefined);
  public readonly dishProduct = input<DishProduct>(undefined);
  public readonly mealType = input<MealType>(undefined);
  public readonly itemIndex = input<number>(undefined);

  private mealCalendarState: Store<MealCalendarState> = inject(Store<MealCalendarState>);
  public portion: number = 0;

  public ngOnInit(): void {
    this.portion = MealCalendarCalculator.calculatePortion(this.dishProduct());
  }

  public onPortionValueChange(customizedPoritonSize: number): void {
    this.mealCalendarState.dispatch(
      MealCalendarActions.updateMealItemPortionRequest({
        request: {
          itemType: ItemType.Dish,
          dishProductId: this.dishProduct().dishProductId,
          itemProductId: this.dish().mealItemId,
          customizedPortionMultiplier: customizedPoritonSize / MealCalendarCalculator.defaultPortionSize,
          date: this.selectedDate(),
        },
      }),
    );
  }
}
