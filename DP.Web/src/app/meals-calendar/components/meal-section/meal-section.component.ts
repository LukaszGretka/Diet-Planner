import { Component, Input, OnInit, inject } from '@angular/core';
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
import { FormsModule } from '@angular/forms';
import { SearchInputComponent } from '../search-input/search-input.component';
import { MealSummaryRowComponent } from './meal-summary-row/meal-summary-row.component';
import { MealDishRowDetailsComponent } from './meal-dish-row-details/meal-dish-row-details.component';
import { MealDishRowDetailsTitleComponent } from './meal-dish-row-details-title/meal-dish-row-details-title.component';

@UntilDestroy()
@Component({
  selector: 'app-meal-section',
  templateUrl: './meal-section.component.html',
  styleUrls: ['./meal-section.component.css'],
  imports: [
    MealItemRowComponent,
    FormsModule,
    SearchInputComponent,
    MealSummaryRowComponent,
    MealDishRowDetailsComponent,
    MealDishRowDetailsTitleComponent,
  ],
})
export class MealSectionComponent implements OnInit {
  private readonly mealCalendarStore = inject<Store<MealCalendarState>>(Store);
  private readonly dishStore = inject<Store<DishState>>(Store);
  private readonly productStore = inject<Store<ProductsState>>(Store);

  @Input() public meal: Meal;
  @Input() calendarDate: Date;

  public searchItem: string;

  public ngOnInit(): void {
    this.dishStore.dispatch(DishActions.loadDishesRequest());
    this.productStore.dispatch(ProductsActions.getAllProductsRequest());
  }

  public itemAddedToSearchBar(item: BaseItem, mealTypeId: MealType) {
    const addMealRequest: MealItemRequest = {
      mealType: mealTypeId,
      date: this.calendarDate,
      mealItemId: item.mealItemId,
      itemType: item.itemType,
      itemId: item.id,
    };

    this.mealCalendarStore.dispatch(MealCalendarActions.addMealRequest({ addMealRequest }));
  }
}
