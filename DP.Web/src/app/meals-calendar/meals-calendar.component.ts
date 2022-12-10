import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { Store } from '@ngrx/store';
import {
  BehaviorSubject,
  debounceTime,
  distinctUntilChanged,
  map,
  Observable,
  OperatorFunction,
  Subscription,
} from 'rxjs';
import { Product } from '../products/models/product';
import { ProductService } from '../products/services/product.service';
import { DailyMealsOverview } from './models/daily-meals-overview';
import { DatePickerSelection } from './models/date-picker-selection';
import { MealType } from './models/meal-type';
import { MealCalendarState } from './stores/meals-calendar.state';
import * as MealCalendarActions from './stores/meals-calendar.actions';
import * as MealCalendarSelectors from './stores/meals-calendar.selectors';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { Meal } from './models/meal';

@UntilDestroy()
@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css'],
})
export class MealsCalendarComponent implements OnInit, OnDestroy {

  public dailyMealsOverview$ = this.store.select(MealCalendarSelectors.getDailyMealsOverview);
  public productsNames$: Observable<string[]> = this.productService
    .getProducts()
    .pipe(map((products) => products.map((product) => product.name)));
  public currentProducts: string[];

  public breakfastProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public lunchProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public dinnerProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public supperProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);

  public dateModel: DatePickerSelection;
  public breakfastSearchModel: string;
  public lunchSearchModel: string;
  public dinnerSearchModel: string;

  public localBreakfastProducts: Product[] = [];
  public localLunchProducts: Product[] = [];
  public localDinnerProducts: Product[] = [];
  public localSupperProducts: Product[] = [];

  private dailyMealsOverviewSub: Subscription;
  private productsNamesSub: Subscription;

  constructor(
    private productService: ProductService,
    private store: Store<MealCalendarState>
  ) { }

  ngOnInit(): void {
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear(),
    };

    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: dateNow }));

    this.dailyMealsOverview$.pipe(
      untilDestroyed(this))
      .subscribe((dailyMealsOverview) => {
        if (!dailyMealsOverview || dailyMealsOverview.length === 0) {
          return;
        }
        const mealTypes = Object.keys(MealType).filter((mealType) => isNaN(Number(mealType)));
        mealTypes.forEach((_, mealTypeId) => this.initializeMealsData(dailyMealsOverview, mealTypeId + 1)
        )
      });

    this.productsNamesSub = this.productsNames$.subscribe((productNames) => {
      this.currentProducts = productNames;
    });
  }

  private initializeMealsData(meals: Meal[], mealType: MealType) {
    const mealProducts = meals.filter(r => r.mealTypeId === mealType)[0]?.products;
    switch (mealType) {
      case MealType.breakfast: {
        this.breakfastProducts$.next(mealProducts);
        mealProducts?.forEach(product => {
          this.localBreakfastProducts.push(product);
        })
      }
      case MealType.lunch: {
        this.lunchProducts$.next(mealProducts);
        mealProducts?.forEach(product => {
          this.localLunchProducts.push(product);
        })
      }
      case MealType.dinner: {
        this.dinnerProducts$.next(mealProducts);
        mealProducts?.forEach(product => {
          this.localDinnerProducts.push(product);
        })
      }
      case MealType.supper: {
        this.supperProducts$.next(mealProducts);
        mealProducts?.forEach(product => {
          this.localSupperProducts.push(product);
        })
      }
    }
  }

  ngOnDestroy(): void {
    this.dailyMealsOverviewSub.unsubscribe();
    this.productsNamesSub.unsubscribe();
  }

  onDateSelection(ngbDate: NgbDate): void {
    const convertedDate = new Date(ngbDate.year + '-' + ngbDate.month + '-' + ngbDate.day);
    this.store.dispatch(MealCalendarActions.getMealsRequest({ date: convertedDate }));
  }

  addBreakfastMeal(): void {
    if (this.breakfastSearchModel) {
      const product = this.productService.getProductByName(this.breakfastSearchModel);
      product.subscribe((product) => {
        if (product) {
          this.localBreakfastProducts.push(product);
          this.breakfastProducts$.next(this.localBreakfastProducts);
        } else {
          // product no exists, want to add a product with this name?
        }
      });
    }
  }

  saveBreakfastMeals(): void {
    this.store.dispatch(MealCalendarActions.addMealRequest({
      mealByDay: {
        date: new Date(),
        products: this.localBreakfastProducts,
        mealTypeId: MealType.breakfast
      }
    }));
  }

  addToLunch(): void {
    if (this.lunchSearchModel) {
      console.log('adding to lunch');
    }
  }

  addToDinner(): void {
    if (this.dinnerSearchModel) {
      console.log('adding to dinner');
    }
  }

  onRemoveButtonClick($event) { }

  searchProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map((searchText) =>
        searchText.length < 1
          ? []
          : this.currentProducts
            .filter((product) => product.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
            .slice(0, 10)
      )
    );
}
