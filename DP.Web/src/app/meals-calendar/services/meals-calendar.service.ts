import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Meal, MealByDay } from '../models/meal';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MealsCalendarService {
  private mealsCalendarUrl = `${environment.dietPlannerApiUri}/api/mealsCalendar`;

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
    return this.httpClient.put<MealByDay>(this.mealsCalendarUrl, mealByDay, this.httpOptions);
  }

  removeDailyMeal(selectedDate: Date, meal: Meal) {
    const dateUTC = selectedDate.toUTCString();
    return this.httpClient.delete<Meal>(this.mealsCalendarUrl + '/' + dateUTC, this.httpOptions);
  }
}
