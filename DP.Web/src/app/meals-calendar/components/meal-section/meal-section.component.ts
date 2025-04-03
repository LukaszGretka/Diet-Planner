import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { BehaviorSubject } from 'rxjs';
import * as MealCalendarActions from '../../stores/meals-calendar.actions';
import { MealCalendarState } from '../../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../../models/meal-type';
import { DishState } from 'src/app/dishes/stores/dish.state';
import * as ProductsActions from '../../../products/stores/products.actions';
import * as DishActions from '../../../dishes/stores/dish.actions';
import { ProductsState } from '../../../products/stores/products.state';
import { BaseItem } from 'src/app/shared/models/base-item';
import { MealItemRequest, Meal } from '../../models/meal';
import { MealItemRowComponent } from './meal-item-row/meal-item-row.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SearchInputComponent } from '../search-input/search-input.component';
import { MealSummaryRowComponent } from './meal-summary-row/meal-summary-row.component';

@UntilDestroy()
@Component({
  selector: 'app-meal-section',
  templateUrl: './meal-section.component.html',
  styleUrls: ['./meal-section.component.css'],
  standalone: true,
  imports: [MealItemRowComponent, CommonModule, FormsModule, SearchInputComponent, MealSummaryRowComponent],
})
export class MealSectionComponent implements OnInit {
  @Input() public meal: Meal;
  @Input() calendarDate: Date;

  public searchItem: string;
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  constructor(
    private mealCalendarStore: Store<MealCalendarState>,
    private dishStore: Store<DishState>,
    private productStore: Store<ProductsState>,
  ) {}

  public ngOnInit(): void {
    this.dishStore.dispatch(DishActions.loadDishesRequest());
    this.productStore.dispatch(ProductsActions.getAllProductsRequest());
  }

  public itemAddedToSearchBar(item: BaseItem, mealTypeId: MealType) {
    const addMealRequest: MealItemRequest = {
      mealType: mealTypeId,
      date: this.calendarDate,
      itemType: item.itemType,
      itemId: item.id,
    };

    this.mealCalendarStore.dispatch(MealCalendarActions.addMealRequest({ addMealRequest }));
  }

  public onPortionValueChange(customizedPortionSize: number, dishId: number, mealDishId: number, productId: number) {
    this.mealCalendarStore.dispatch(
      DishActions.updatePortionRequest({
        dishId: dishId,
        productId: productId,
        mealDishId: mealDishId,
        customizedPortionMultiplier: customizedPortionSize / this.defaultPortionSize,
        date: this.calendarDate,
      }),
    );
  }
}
