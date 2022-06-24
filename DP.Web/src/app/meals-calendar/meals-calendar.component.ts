import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
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
import { MealsCalendarService } from './services/meals-calendar.service';

@Component({
	selector: 'app-meals-calendar',
	templateUrl: './meals-calendar.component.html',
	styleUrls: ['./meals-calendar.component.css'],
})
export class MealsCalendarComponent implements OnInit, OnDestroy {
	public dailyMealsOverview$: Observable<DailyMealsOverview>;
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

	public localBreakfastProducts: Product[];

	private dailyMealsOverviewSub: Subscription;
	private productsNamesSub: Subscription;

	constructor(
		private mealsCalendarService: MealsCalendarService,
		private productService: ProductService
	) {}

	ngOnInit(): void {
		const dateNow = new Date();

		this.dateModel = {
			day: dateNow.getDate(),
			month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
			year: dateNow.getFullYear(),
		};
		this.dailyMealsOverviewSub = this.mealsCalendarService
			.getDailyMeals(dateNow)
			.subscribe((dailyMealsOverview) => {
				this.localBreakfastProducts = dailyMealsOverview.breakfast?.products;
				// this.localLunchProducts = dailyMealsOverview.lunch?.products;
				// this.localDinnerProducts = dailyMealsOverview.dinner?.products;
				// this.localSupperProducts = dailyMealsOverview.supper?.products;

				this.breakfastProducts$.next(dailyMealsOverview.breakfast?.products);
				this.lunchProducts$.next(dailyMealsOverview.lunch?.products);
				this.dinnerProducts$.next(dailyMealsOverview.dinner?.products);
				this.supperProducts$.next(dailyMealsOverview.supper?.products);
			});

		this.productsNamesSub = this.productsNames$.subscribe((productNames) => {
			this.currentProducts = productNames;
		});
	}

	ngOnDestroy(): void {
		this.dailyMealsOverviewSub.unsubscribe();
		this.productsNamesSub.unsubscribe();
	}

	onDateSelection(ngbDate: NgbDate): void {
		const convertedDate = new Date(ngbDate.year, ngbDate.month, ngbDate.day);
		this.dailyMealsOverview$ = this.mealsCalendarService.getDailyMeals(convertedDate);
	}

	addToBreakfast(): void {
		if (this.breakfastSearchModel) {
			const product = this.productService.getProductByName(this.breakfastSearchModel);
			product.subscribe((product) => {
				if (product) {
					this.localBreakfastProducts.push(product);
				} else {
					// product no exists, want to add a product with this name?
				}
			});
		}
	}

	saveBreafastMeals(): void {
		this.mealsCalendarService.addDialyMeal({
			date: new Date(this.dateModel.year, this.dateModel.month, this.dateModel.day).toISOString(),
			products: this.localBreakfastProducts,
			mealType: MealType.breakfast,
		});
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

	onRemoveButtonClick($event) {}

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
