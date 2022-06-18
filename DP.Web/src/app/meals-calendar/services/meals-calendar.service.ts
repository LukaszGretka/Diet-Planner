import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from 'src/app/products/models/product';
import { DailyMealsOverview } from '../models/daily-meals-overview';
import { Meal } from '../models/meal';
import { MealType } from '../models/meal-type';

@Injectable({
  providedIn: 'root',
})
export class MealsCalendarService {
  private mealsCalendarUrl = 'http://localhost:5000/api/mealsCalendar';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  getDailyMeals(selectedDate: Date): Observable<DailyMealsOverview> {
    return this.httpClient.get<DailyMealsOverview>(
      this.mealsCalendarUrl + '/' + selectedDate.toISOString()
    );
  }

  addDialyMeal(selectedDate: Date, meal: Meal) {
    return this.httpClient.post<Meal>(
      this.mealsCalendarUrl + `/${selectedDate}`,
      meal,
      this.httpOptions
    );
  }
  removeDailyMeal(selectedDate: Date, meal: Meal) {
    return this.httpClient.delete<Meal>(
      this.mealsCalendarUrl + `/${selectedDate}`,
      this.httpOptions
    );
  }
}
