import { Component, OnInit } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { BehaviorSubject, Observable } from 'rxjs';
import { Product } from '../products/models/product';
import { DailyMeals } from './models/daily-meals';
import { DatePickerSelection } from './models/date-picker-selection';
import { MealsCalendarService } from './services/meals-calendar.service';

@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css']
})
export class MealsCalendarComponent implements OnInit {

  public dailyMeals$: Observable<DailyMeals>;

  public breakfastProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public lunchProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public dinnerProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);
  public supperProducts$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);

  public dateModel: DatePickerSelection;

  constructor(private mealsCalendarService: MealsCalendarService) { }

  ngOnInit(): void {
    const dateNow = new Date();

    this.dateModel = {
      day: dateNow.getDate(),
      month: dateNow.getMonth() + 1, // getMonth method is off by 1. (0-11)
      year: dateNow.getFullYear()
    }
    //TODO: Next step is to send proper date to controller.
    this.dailyMeals$ = // here we should use stream from selector
      this.mealsCalendarService.getDailyMeals(dateNow);
    this.dailyMeals$.subscribe(dailyMeals => {
      this.breakfastProducts$.next(dailyMeals.breakfast?.products);
      this.lunchProducts$.next(dailyMeals.lunch?.products);
      this.dinnerProducts$.next(dailyMeals.dinner?.products);
      this.supperProducts$.next(dailyMeals.supper?.products);
    })
  }

  onDateSelection(ngbDate: NgbDate): void {
    const convertedDate = new Date(ngbDate.year, ngbDate.month, ngbDate.day)
    this.dailyMeals$ = this.mealsCalendarService.getDailyMeals(convertedDate);
  }

  addToBreakfast(): void {
    console.log('adding to breakfast');
  }

  addToLunch(): void {
    console.log('adding to lunch');
  }


  addToDinner(): void {
    console.log('adding to dinner');
  }


  onRemoveButtonClick($event) {

  }
}
