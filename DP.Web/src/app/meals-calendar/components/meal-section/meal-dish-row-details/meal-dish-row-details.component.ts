import { CommonModule, DecimalPipe, NgIf } from '@angular/common';
import { Component, inject, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Dish } from 'src/app/dishes/models/dish';
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { DishState } from 'src/app/dishes/stores/dish.state';
import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { MealCalendarState } from 'src/app/meals-calendar/stores/meals-calendar.state';
import * as MealCalendarActions from '../../../stores/meals-calendar.actions';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';

@Component({
  selector: '[app-meal-dish-row-details]',
  templateUrl: 'meal-dish-row-details.component.html',
  standalone: true,
  imports: [DecimalPipe, CommonModule, FormsModule],
})
export class MealDishRowDetailsComponent implements OnInit {
  @Input() public dish: Dish;
  @Input() public selectedDate: Date;
  @Input() public dishProduct: DishProduct;
  @Input() public mealType: MealType;
  @Input() public itemIndex: number;

  private mealCalendarState: Store<MealCalendarState> = inject(Store<MealCalendarState>);
  public portion: number = 0;

  public ngOnInit(): void {
    this.portion = MealCalendarCalculator.calculatePortion(this.dishProduct);
  }

  public onPortionValueChange(customizedPoritonSize: number): void {
    this.mealCalendarState.dispatch(
      MealCalendarActions.updateMealItemPortionRequest({
        request: {
          itemProductId: this.dish.mealItemId,
          customizedPortionMultiplier: customizedPoritonSize / MealCalendarCalculator.defaultPortionSize,
          date: this.selectedDate,
        },
      }),
    );
  }
}
