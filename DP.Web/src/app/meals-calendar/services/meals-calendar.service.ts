import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Meal, MealByDay } from '../models/meal';
import { MealType } from '../models/meal-type';

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
    const dateUTC = selectedDate.toUTCString();
    return this.httpClient.get<Meal[]>(this.mealsCalendarUrl + '/' + dateUTC, {
      withCredentials: true,
    });
  }

  addDialyMeal(mealByDay: MealByDay): Observable<MealByDay> {
    return this.httpClient.post<MealByDay>(this.mealsCalendarUrl, mealByDay, this.httpOptions);
  }

  removeDailyMeal(selectedDate: Date, meal: Meal) {
    const dateUTC = selectedDate.toUTCString();
    return this.httpClient.delete<Meal>(this.mealsCalendarUrl + '/' + dateUTC, this.httpOptions);
  }
}
