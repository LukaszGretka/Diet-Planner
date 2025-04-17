import { DecimalPipe } from '@angular/common';
import { Component, Input, OnInit, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { MealCalendarCalculator } from 'src/app/meals-calendar/services/meal-calendar-calculator.service';
import { BaseItem, ItemType } from 'src/app/shared/models/base-item';
import { Store } from '@ngrx/store';
import * as MealCalendarSelectors from 'src/app/meals-calendar/stores/meals-calendar.selectors';
import { MealCalendarState } from '../../../stores/meals-calendar.state';
import { take } from 'rxjs';
import * as MealCalendarActions from '../../../stores/meals-calendar.actions';
import { MealType } from 'src/app/meals-calendar/models/meal-type';
import { MealRowDetails } from 'src/app/meals-calendar/models/meal-dish-row-details';
import { FormsModule } from '@angular/forms';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: '[app-meal-item-row]',
  templateUrl: './meal-item-row.component.html',
  imports: [DecimalPipe, FormsModule],
})
export class MealItemRowComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly mealCalendarStore = inject<Store<MealCalendarState>>(Store);

  @Input() public item: BaseItem;
  public readonly calendarDate = input<Date>(undefined);
  public readonly mealType = input<MealType>(undefined);
  public readonly itemIndex = input<number>(undefined);

  public mealRowDetails: MealRowDetails;

  public ngOnInit(): void {
    this.mealCalendarStore
      .select(MealCalendarSelectors.getMealByMealType(this.mealType()))
      .pipe(untilDestroyed(this))
      .subscribe(meal => {
        if (meal) {
          this.mealRowDetails = this.calculateMealRowDetails();
        }
      });
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
          mealType: this.mealType(),
          mealItemId: this.item.mealItemId,
          date: this.calendarDate(),
          itemType: this.item.itemType,
          itemId: this.item.id,
        },
      }),
    );
  }

  //For product portion changes as it's editable on main item row
  public onPortionValueChange(customizedPoritonSize: number): void {
    this.mealCalendarStore.dispatch(
      MealCalendarActions.updateMealItemPortionRequest({
        request: {
          itemType: ItemType.Product,
          itemProductId: this.item.mealItemId,
          customizedPortionMultiplier: customizedPoritonSize / MealCalendarCalculator.defaultPortionSize,
          date: this.calendarDate(),
        },
      }),
    );
  }

  public calculateNutritionalValue(value: number, portion: number, itemType: number): string {
    return itemType === 0 ? (value * portion).toFixed(1) : value.toFixed(1);
  }

  private calculateMealRowDetails(): MealRowDetails {
    let result: MealRowDetails;
    if (this.item.itemType === ItemType.Dish) {
      this.mealCalendarStore
        .select(MealCalendarSelectors.getMealDishById(this.item.mealItemId))
        .pipe(take(1))
        .subscribe(dish => {
          if (dish) {
            result = MealCalendarCalculator.calculateMealDishRowDetails(dish);
          }
        });
    } else if (this.item.itemType === ItemType.Product) {
      this.mealCalendarStore
        .select(MealCalendarSelectors.getMealProductById(this.item.mealItemId))
        .pipe(take(1))
        .subscribe(product => {
          if (product) {
            result = MealCalendarCalculator.calculateMealProductRowDetails(product);
          }
        });
    }

    return result;
  }
}
