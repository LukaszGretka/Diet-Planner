import { CommonModule, DecimalPipe, NgIf } from '@angular/common';
import { Component, inject, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Dish } from 'src/app/dishes/models/dish';
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { DishState } from 'src/app/dishes/stores/dish.state';
import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { MealCalendarState } from 'src/app/meals-calendar/stores/meals-calendar.state';
import * as MealCalendarActions from '../../../stores/meals-calendar.actions';

@Component({
  selector: '[app-meal-dish-row-details]',
  templateUrl: 'meal-dish-row-details.component.html',
  standalone: true,
  imports: [DecimalPipe, CommonModule, FormsModule],
})
export class MealDishRowDetailsComponent {
  @Input() public dish: Dish;
  @Input() public selectedDate: Date;
  @Input() public dishProduct: DishProduct;
  @Input() public mealType: MealType;
  @Input() public itemIndex: number;


  private mealCalendarState: Store<MealCalendarState> = inject(Store<MealCalendarState>);
  public defaultPortionSize = 100; //in grams

  public onPortionValueChange(customizedPoritonSize: number, dishId: number, mealDishId: number, productId: number) {
    this.mealCalendarState.dispatch(
      MealCalendarActions.updateMealItemPortionRequest({
        request: {
          dishId: dishId,
          productId: productId,
          mealDishId: mealDishId,
          customizedPortionMultiplier: customizedPoritonSize / this.defaultPortionSize,
          date: this.selectedDate,
        },
      }),
    );
  }
}
