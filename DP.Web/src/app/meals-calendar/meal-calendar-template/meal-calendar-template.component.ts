import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map, take } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import * as DishActions from './../../dishes/stores/dish.actions';
import { ProductService } from 'src/app/products/services/product.service';
import { MealCalendarState } from '../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../models/meal-type';
import { Meal } from '../models/meal';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as MealCalendarSelectors from './../stores/meals-calendar.selectors';
import * as ProductsActions from '../../products/stores/products.actions';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as ProductSelectors from './../../products/stores/products.selectors';
import { DishProduct } from 'src/app/dishes/models/dish-product';
import { DishState } from 'src/app/dishes/stores/dish.state';
import * as DishSelectors from './../../dishes/stores/dish.selectors';
import { Dish } from 'src/app/dishes/models/dish';

@UntilDestroy()
@Component({
  selector: 'app-meal-calendar-template',
  templateUrl: './meal-calendar-template.component.html',
  styleUrls: ['./meal-calendar-template.component.css'],
})
export class MealCalendarTemplateComponent implements OnInit {
  @Input()
  public dishes$: BehaviorSubject<Dish[]>;

  @Input()
  public selectedDate: Date;

  @Input()
  public mealType: MealType;

  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.store.select(ProductSelectors.getCallbackMealProduct);
  public allDishes$ = this.dishStore.select(DishSelectors.getDishes);

  public searchItem: string;
  public defaultPortionSize = 100; //in grams
  public portionValue = this.defaultPortionSize;

  public mealMacroSummary$: Observable<any>;

  constructor(
    private store: Store<MealCalendarState>,
    private dishStore: Store<DishState>,
    private router: Router,
    private modalService: NgbModal,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.dishStore.dispatch(DishActions.loadDishesRequest());
    this.store.dispatch(ProductsActions.getAllProductsRequest());
    this.mealMacroSummary$ = this.dishes$.pipe(
      map(dishes =>
        dishes.reduce(
          (total, dish: Dish) => {
            dish.products.forEach((dishProduct: DishProduct) => {
              (total.calories += dishProduct.product.calories),
                (total.carbohydrates += dishProduct.product.carbohydrates),
                (total.proteins += dishProduct.product.proteins),
                (total.fats += dishProduct.product.fats);
            });
            return total;
          },
          { calories: 0, carbohydrates: 0, proteins: 0, fats: 0 },
        ),
      ),
    );
  }

  // Update local products list for particular collection given in parameter.
  public onAddProductButtonClick(behaviorSubject: BehaviorSubject<any>, dishName: string, content: any): void {
    if (dishName) {
      const foundDishes: Dish[] = this.searchForDishByName(dishName);
      if (foundDishes) {
        this.addFoundDish(behaviorSubject, foundDishes);
        this.searchItem = '';
      } else {
        this.modalService.open(content);
      }
    }
  }

  public onCancelModalClick() {
    this.modalService.dismissAll();
  }

  // Remove dish from local list by given index
  public onRemoveDishButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const dishesBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dishes = dishesBehaviorSubject.getValue();
    let localDishes = [...dishes];
    localDishes.splice(index, 1);
    dishesBehaviorSubject.next(localDishes);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          dishes: dishesBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public addNewProductModalButtonClick(): void {
    this.modalService.dismissAll();
    this.store.dispatch(
      ProductsActions.setCallbackMealProduct({ productName: this.searchItem, mealType: this.mealType }),
    );
    this.router.navigateByUrl(`products/add?redirectUrl=${this.router.url}`);
  }

  public onEditDishButtonClick(index: number): void {
    // to be implemented
  }

  public searchDishOrProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(searchText =>
        searchText.length < 1
          ? []
          : this.searchForDishByName(searchText)
              .slice(0, 10)
              .map(dish => dish.name),
      ),
    );

  private searchForDishByName(searchText: string): Dish[] {
    let foundDishes: Dish[];
    this.allDishes$.pipe(take(1)).subscribe(dishes => {
      foundDishes = dishes.filter(dish => dish.name.toLowerCase().indexOf(searchText.toLowerCase()) > -1);
    });

    return foundDishes;
  }

  private addFoundDish(behaviorSubject: BehaviorSubject<any>, foundDishes: Dish[]): void {
    const dishBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dishes = (dishBehaviorSubject.getValue() as Dish[]).concat(foundDishes);
    dishBehaviorSubject.next(dishes);
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          dishes: dishBehaviorSubject.getValue(),
          mealTypeId: this.mealType,
        },
      }),
    );
  }

  public onPortionValueChange(poritonSize: number, dishId: number, productId: number) {
    this.store.dispatch(
      DishActions.updatePortionRequest({
        dishId: dishId,
        productId: productId,
        portionMultiplier: poritonSize / this.defaultPortionSize,
        date: this.selectedDate,
      }),
    );
  }

  public calculateDishMacros(dish: Dish): { carbs: number; proteins: number; fats: number; calories: number } {
    let dishMacro = { carbs: 0, proteins: 0, fats: 0, calories: 0 };
    dish.products.forEach(p => {
      dishMacro.carbs += p.product.carbohydrates;
      dishMacro.proteins += p.product.proteins;
      dishMacro.fats += p.product.fats;
      dishMacro.calories += p.product.calories;
    });

    return dishMacro;
  }
}
