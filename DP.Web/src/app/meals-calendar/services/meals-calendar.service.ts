import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DailyMealsOverview } from '../models/daily-meals-overview';
import { Meal, MealByDay } from '../models/meal';

@Injectable({
  providedIn: 'root',
})
export class MealsCalendarService {
  private mealsCalendarUrl = 'http://localhost:5000/api/mealsCalendar';

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  constructor(private httpClient: HttpClient) {}

  getDailyMeals(selectedDate: Date): Observable<Meal[]> {
    return this.httpClient.get<Meal[]>(this.mealsCalendarUrl + '/' + selectedDate.toDateString(), {
      withCredentials: true,
    });
  }

  addDialyMeal(mealByDay: MealByDay): Observable<MealByDay> {
    return this.httpClient.post<MealByDay>(this.mealsCalendarUrl, mealByDay, this.httpOptions);
  }

  removeDailyMeal(selectedDate: Date, meal: Meal) {
    return this.httpClient.delete<Meal>(this.mealsCalendarUrl + `/${selectedDate.toDateString()}`, this.httpOptions);
  }
}
