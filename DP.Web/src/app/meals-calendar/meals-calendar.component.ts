import {Component, OnInit} from '@angular/core';
import {NgbDate} from '@ng-bootstrap/ng-bootstrap';
import {Store} from '@ngrx/store';
import {BehaviorSubject, debounceTime, distinctUntilChanged, map, Observable, OperatorFunction} from 'rxjs';
import {Product} from '../products/models/product';
import {ProductService} from '../products/services/product.service';
import {DatePickerSelection} from './models/date-picker-selection';
import {MealType} from './models/meal-type';
import {MealCalendarState} from './stores/meals-calendar.state';
import * as MealCalendarActions from './stores/meals-calendar.actions';
import * as MealCalendarSelectors from './stores/meals-calendar.selectors';
import * as GeneralSelector from './../stores/store.selectors';
import {UntilDestroy, untilDestroyed} from '@ngneat/until-destroy';

@UntilDestroy()
@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css'],
})
export class MealsCalendarComponent implements OnInit {
  //TODO move to effect
  //may require refactor if list of products will be long (need to test it)
  public productsNames$: Observable<string[]> = this.productService
    .getProducts()
    .pipe(map(products => products.map(product => product.name)));

  public currentProducts: string[];

  public dailyMealsOverview$ = this.store.select(MealCalendarSelectors.getDailyMealsOverview);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  public breakfastProducts$ = new BehaviorSubject([]);
  public lunchProducts$ = new BehaviorSubject([]);
  public dinnerProducts$ = new BehaviorSubject([]);
  public supperProducts$ = new BehaviorSubject([]);

  public dateModel: DatePickerSelection;
  public breakfastSearchModel: string;
  public lunchSearchModel: string;
  public dinnerSearchModel: string;

  private selectedDate: Date;

  constructor(private productService: ProductService, private store: Store<MealCalendarState>) {}

  ngOnInit(): void {
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear(),
    };

    this.selectedDate = dateNow;
    this.store.dispatch(MealCalendarActions.getMealsRequest({date: this.selectedDate}));

    this.dailyMealsOverview$.pipe(untilDestroyed(this)).subscribe(meals => {
      this.breakfastProducts$.next(meals.filter(m => m.mealTypeId === MealType.breakfast)[0]?.products ?? []);
      this.lunchProducts$.next(meals.filter(m => m.mealTypeId === MealType.lunch)[0]?.products ?? []);
      this.dinnerProducts$.next(meals.filter(m => m.mealTypeId === MealType.dinner)[0]?.products ?? []);
      this.supperProducts$.next(meals.filter(m => m.mealTypeId === MealType.supper)[0]?.products ?? []);
    });

    this.productsNames$.pipe(untilDestroyed(this)).subscribe(productNames => {
      this.currentProducts = productNames;
    });
  }

  onDateSelection(ngbDate: NgbDate): void {
    this.selectedDate = new Date(ngbDate.year + '-' + ngbDate.month + '-' + ngbDate.day);
    this.store.dispatch(MealCalendarActions.getMealsRequest({date: this.selectedDate}));
  }

  // Update local products list for particular collection given in parameter.
  public onAddProductButtonClick(behaviorSubject: BehaviorSubject<any>, productName: string): void {
    if (productName) {
      this.productService
        .getProductByName(productName)
        .pipe(untilDestroyed(this))
        .subscribe(product => {
          if (product) {
            const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
            const products = (productsBehaviorSubject.getValue() as Product[]).concat(product);
            productsBehaviorSubject.next(products);
          } else {
            //TODO: product no exists, want to add a product with this name?
          }
        });
    }
  }

  // Remove product from local list by given index
  public onRemoveProductButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {
    const productsBehaviorSubject = behaviorSubject as BehaviorSubject<Product[]>;
    const products = productsBehaviorSubject.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    productsBehaviorSubject.next(productsLocal);
  }

  public onMealSaveButtonClick(behaviorSubject: BehaviorSubject<any>, mealType: MealType) {
    this.store.dispatch(
      MealCalendarActions.addMealRequest({
        mealByDay: {
          date: this.selectedDate,
          products: behaviorSubject.getValue(),
          mealTypeId: mealType,
        },
      }),
    );
  }

  searchProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(searchText =>
        searchText.length < 1
          ? []
          : this.currentProducts
              .filter(product => product.toLowerCase().indexOf(searchText.toLowerCase()) > -1)
              .slice(0, 10),
      ),
    );
}
