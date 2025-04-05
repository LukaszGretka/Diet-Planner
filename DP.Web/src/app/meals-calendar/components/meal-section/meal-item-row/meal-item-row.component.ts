import { DecimalPipe, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MacronutrientsWithCalorties } from 'src/app/meals-calendar/models/macronutrients';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';
import { BaseItem, ItemType } from 'src/app/shared/models/base-item';
import { Store } from '@ngrx/store';
import * as MealCalendarSelectors from 'src/app/meals-calendar/stores/meals-calendar.selectors';
import { MealCalendarState } from '../../../stores/meals-calendar.state';
import { take } from 'rxjs';
import * as MealCalendarActions from '../../../stores/meals-calendar.actions';
import { MealType } from 'src/app/meals-calendar/models/meal-type';

@Component({
  selector: '[app-meal-item-row]',
  templateUrl: './meal-item-row.component.html',
  standalone: true,
  imports: [DecimalPipe, NgIf],
})
export class MealItemRowComponent implements OnInit {
  @Input() public item: BaseItem;
  @Input() public calendarDate: Date;
  @Input() public mealType: MealType;
  @Input() public itemIndex: number;

  public macrosWithCalories: MacronutrientsWithCalorties;

  public constructor(private router: Router, private mealCalendarStore: Store<MealCalendarState>) {}

  public ngOnInit(): void {
    this.macrosWithCalories = this.calculateMacrosWithCalories();
  }

  public async onEditItemButtonClick(): Promise<void> {
    await this.router.navigateByUrl(
      `${ItemType[this.item.itemType].toLocaleLowerCase()}/edit/${this.item.id}?returnUrl=${this.router.url}`,
    );
  }

  public onRemoveItemButtonClick(): void {
    this.mealCalendarStore.dispatch(
      MealCalendarActions.removeMealItemRequest({
        removeMealRequest: {
          mealType: this.mealType,
          date: this.calendarDate,
          itemType: this.item.itemType,
          itemId: this.item.id,
        },
      }),
    );
  }

  private calculateMacrosWithCalories(): MacronutrientsWithCalorties {
    let result: MacronutrientsWithCalorties;
    if (this.item.itemType === ItemType.Dish) {
      this.mealCalendarStore
        .select(MealCalendarSelectors.getMealDishById(this.item.id))
        .pipe(take(1))
        .subscribe(dish => {
          if (dish) {
            result = MealCalendarCalculator.calculateDishProductsMacros(dish);
          }
        });
    } else if (this.item.itemType === ItemType.Product) {
      this.mealCalendarStore
        .select(MealCalendarSelectors.getMealProductById(this.item.id))
        .pipe(take(1))
        .subscribe(product => {
          if (product) {
            result = MealCalendarCalculator.calculateProductMacros(product);
          }
        });
    }

    return result;
  }
}
