import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { BehaviorSubject, Observable } from 'rxjs';
import { Product } from '../products/models/product';
import { GeneralState } from '../stores/store.state';
import { DailyMeals } from './models/daily-meals';
import { DatePickerSelection } from './models/date-picker-selection';
import { SpecifiedMeal } from './models/specified-meal';
import { MealsCalendarService } from './services/meals-calendar.service';

@Component({
  selector: 'app-meals-calendar',
  templateUrl: './meals-calendar.component.html',
  styleUrls: ['./meals-calendar.component.css']
})
export class MealsCalendarComponent implements OnInit {

  public dailyMeals$: Observable<DailyMeals>;
  public breakfastMeals$: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>(null);

  public dateModel: DatePickerSelection;

  constructor(private mealsCalendarService: MealsCalendarService) { }

  ngOnInit(): void {
    const dateNow = new Date();
    this.dateModel = {
      day: dateNow.getDay(),
      month: dateNow.getMonth(),
      year: dateNow.getFullYear()
    }

    this.dailyMeals$ = this.mealsCalendarService.getDailyMeals(dateNow);
    this.dailyMeals$.subscribe(dailyMeals => {
      this.breakfastMeals$.next(dailyMeals.breakfast?.products);
      // Add more of dialy meals
    })
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
