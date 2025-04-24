import { Component, inject, output } from '@angular/core';
import {
  combineLatest,
  debounceTime,
  distinctUntilChanged,
  map,
  Observable,
  of,
  OperatorFunction,
  switchMap,
  take,
} from 'rxjs';
import * as DishSelectors from '../../../dishes/stores/dish.selectors';
import * as ProductSelectors from '../../../products/stores/products.selectors';
import { Store } from '@ngrx/store';
import { DishState } from '../../../dishes/stores/dish.state';
import { ProductsState } from '../../../products/stores/products.state';
import { NgbModal, NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';
import { BaseItem, ItemType } from '../../../shared/models/base-item';
import { Dish } from '../../../dishes/models/dish';
import { Product } from '../../../products/models/product';
import { FormsModule } from '@angular/forms';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-search-input',
  templateUrl: './search-input.component.html',
  styleUrl: './search-input.component.css',
  imports: [NgbTypeahead, FormsModule],
})
export class SearchInputComponent {
  private readonly dishStore = inject<Store<DishState>>(Store);
  private readonly productStore = inject<Store<ProductsState>>(Store);
  private readonly modalService = inject(NgbModal);
  private readonly notificationService = inject(NotificationService);

  public readonly itemAddedEmitter = output<BaseItem>();

  //TODO: taking list of dishes might be long (need to find better solution)
  public allDishes$: Observable<Dish[]> = this.dishStore.select(DishSelectors.getDishes);
  public allProducts$: Observable<Product[]> = this.productStore.select(ProductSelectors.getAllProducts);

  public searchItem: BaseItem;
  baseItemFormatter = (item: BaseItem) => `${item.name} (${ItemType[item.itemType]})`;

  public search: OperatorFunction<string, readonly BaseItem[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(searchText => {
        if (searchText.length < 2) {
          return of([]);
        }

        return combineLatest([this.allProducts$, this.allDishes$]).pipe(
          map(([products, dishes]) => {
            const productNames = products.filter(
              product => product.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1,
            );
            const dishNames = dishes.filter(dish => dish.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1);
            return [...productNames, ...dishNames];
          }),
        );
      }),
    );

  public onAddSearchItemAdd(searchItem: BaseItem): void {
    if (!searchItem) {
      return;
    }

    const itemsCollection = combineLatest([this.allProducts$, this.allDishes$]);

    itemsCollection.pipe(take(1)).subscribe(([products, dishes]) => {
      const foundItem = this.findItemById(searchItem, products, dishes);
      if (foundItem) {
        this.itemAddedEmitter.emit(foundItem);
        this.searchItem = null;
      } else {
        this.notificationService.showErrorToast('Error', `Item '${searchItem}' not found.`);
      }
    });
  }

  private findItemById(searchItem: BaseItem, products: Product[], dishes: Dish[]): BaseItem | null {
    if (searchItem.itemType === ItemType.Product) {
      return products.find(product => product.id === searchItem.id) || null;
    } else if (searchItem.itemType === ItemType.Dish) {
      return dishes.find(dish => dish.id === searchItem.id) || null;
    }
    return null;
  }

  public async addNewDishModalButtonClick(): Promise<void> {
    this.modalService.dismissAll();
    //this.store.dispatch(DishActions.setCallbackMealDish({ dishName: this.searchItem, mealType: this.mealType }));
    //await this.router.navigateByUrl(`dishes/add?returnUrl=${this.router.url}`);
  }

  public onCancelModalClick() {
    this.searchItem = null;
    this.modalService.dismissAll();
  }
}
