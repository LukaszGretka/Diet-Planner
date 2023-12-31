import { Component, Input, OnInit } from '@angular/core';
import { UntilDestroy } from '@ngneat/until-destroy';
import { BehaviorSubject, Observable, OperatorFunction, debounceTime, distinctUntilChanged, map, take } from 'rxjs';
import * as MealCalendarActions from './../stores/meals-calendar.actions';
import * as DishActions from './../../dishes/stores/dish.actions';
import { MealCalendarState } from '../stores/meals-calendar.state';
import { Store } from '@ngrx/store';
import { MealType } from '../models/meal-type';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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
  // public allProducts$ = this.store.select(ProductSelectors.getCallbackMealProduct);
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
  ) {}

  ngOnInit(): void {
    this.dishStore.dispatch(DishActions.loadDishesRequest());
    //this.store.dispatch(ProductsActions.getAllProductsRequest());
    this.mealMacroSummary$ = this.dishes$.pipe(
      map(dishes =>
        dishes.reduce(
          (total, dish: Dish) => {
            dish.products.forEach((dishProduct: DishProduct) => {
              (total.calories += dishProduct.product.calories * dishProduct.customizedPortionMultiplier),
                (total.carbohydrates += dishProduct.product.carbohydrates * dishProduct.customizedPortionMultiplier),
                (total.proteins += dishProduct.product.proteins * dishProduct.customizedPortionMultiplier),
                (total.fats += dishProduct.product.fats * dishProduct.customizedPortionMultiplier);
            });
            return total;
          },
          { calories: 0, carbohydrates: 0, proteins: 0, fats: 0 },
        ),
      ),
    );
  }

  // Update local products list for particular collection given in parameter.
  public onAddDishOrProductButtonClick(behaviorSubject: BehaviorSubject<any>, dishName: string, content: any): void {
    if (dishName) {
      const foundDishes: Dish[] = this.searchForDishByName(dishName);
      if (foundDishes.length > 0) {
        this.addFoundDish(behaviorSubject, foundDishes);
        this.searchItem = '';
      } else {
        this.modalService.open(content);
      }
    }
  }

  public onCancelModalClick() {
    this.searchItem = '';
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

  public addNewDishModalButtonClick(): void {
    this.modalService.dismissAll();
    this.store.dispatch(DishActions.setCallbackMealDish({ dishName: this.searchItem, mealType: this.mealType }));
    this.router.navigateByUrl(`dishes/dish-add?redirectUrl=${this.router.url}`);
  }

  public onEditDishButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const dishesBehaviorSubject = behaviorSubject as BehaviorSubject<Dish[]>;
    const dish: Dish = dishesBehaviorSubject.getValue()[index];
    this.router.navigateByUrl(`dishes/edit/${dish.id}?redirectUrl=${this.router.url}`);
  }

  public searchDish: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
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

  public onPortionValueChange(customizedPoritonSize: number, dishId: number, productId: number) {
    this.store.dispatch(
      DishActions.updatePortionRequest({
        dishId: dishId,
        productId: productId,
        customizedPortionMultiplier: customizedPoritonSize / this.defaultPortionSize,
        date: this.selectedDate,
      }),
    );
  }

  public calculateDishMacros(dish: Dish): { carbs: number; proteins: number; fats: number; calories: number } {
    let dishMacro = { carbs: 0, proteins: 0, fats: 0, calories: 0 };
    dish.products.forEach(dishProduct => {
      dishMacro.carbs += dishProduct.product.carbohydrates * dishProduct.customizedPortionMultiplier;
      dishMacro.proteins += dishProduct.product.proteins * dishProduct.customizedPortionMultiplier;
      dishMacro.fats += dishProduct.product.fats * dishProduct.customizedPortionMultiplier;
      dishMacro.calories += dishProduct.product.calories * dishProduct.customizedPortionMultiplier;
    });

    return dishMacro;
  }
}
