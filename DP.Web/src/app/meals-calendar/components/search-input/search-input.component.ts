import { Component, EventEmitter, Output } from '@angular/core';
import {
  BehaviorSubject,
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  map,
  Observable,
  of,
  OperatorFunction,
  switchMap,
} from 'rxjs';
import * as DishSelectors from '../../../dishes/stores/dish.selectors';
import * as ProductSelectors from '../../../products/stores/products.selectors';
import { Store } from '@ngrx/store';
import { DishState } from '../../../dishes/stores/dish.state';
import { ProductsState } from '../../../products/stores/products.state';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseItem, ItemType } from '../../../shared/models/base-item';
import { Dish } from '../../../dishes/models/dish';
import { Product } from '../../../products/models/product';
import * as MealCalendarActions from '../../stores/meals-calendar.actions';
import { MealCalendarState } from '../../stores/meals-calendar.state';

@Component({
  selector: 'app-search-input',
  templateUrl: './search-input.component.html',
  styleUrl: './search-input.component.css'
})
export class SearchInputComponent {

  @Output()
  public itemAddedEmitter = new EventEmitter<BaseItem>();

  //TODO: taking list of dishes might be long (need to find better solution)
  public allDishes$: Observable<Dish[]> = this.dishStore.select(DishSelectors.getDishes);
  public allProducts$: Observable<Product[]> = this.productStore.select(ProductSelectors.getAllProducts);

  public searchItem: BaseItem;
  baseItemFormatter = (item: BaseItem) => `${item.name} (${ItemType[item.itemType]})`;

  constructor(private dishStore: Store<DishState>, private productStore: Store<ProductsState>, private modalService: NgbModal,
              private store: Store<MealCalendarState>) {
  }

  public search: OperatorFunction<string, readonly BaseItem[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(searchText => {
        if(searchText.length < 2){
          return of([]);
        }

        return combineLatest([this.allProducts$, this.allDishes$]).pipe(
          map(([products, dishes]) => {
            const productNames = products.filter(product => product.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1);
            const dishNames = dishes.filter(dish => dish.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1);
            return [...productNames, ...dishNames];
          })
        );
      }));

  // Update local products list for particular collection given in parameter.
  public onAddDishOrProductButtonClick(searchItem: BaseItem): void {
    this.itemAddedEmitter.emit(searchItem);

      // const foundDishes: Dish[] = this.searchForDishByName(dishName);
      // if (foundDishes.length > 0) {
      //   this.addFoundDish(behaviorSubject, foundDishes);
      //   this.searchItem = '';
      // } else {
      //   this.modalService.open(content);
      // }

  }


  public async addNewDishModalButtonClick(): Promise<void> {
    this.modalService.dismissAll();
    //this.store.dispatch(DishActions.setCallbackMealDish({ dishName: this.searchItem, mealType: this.mealType }));
    //await this.router.navigateByUrl(`dishes/dish-add?redirectUrl=${this.router.url}`);
  }

  public onCancelModalClick() {
    this.searchItem = null;
    this.modalService.dismissAll();
  }
}
