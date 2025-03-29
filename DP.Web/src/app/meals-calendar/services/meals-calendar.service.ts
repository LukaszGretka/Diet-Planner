import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddMealItemRequest, Meal } from '../models/meal';
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

  public constructor(private httpClient: HttpClient) {}

  // Gets all meals for selected date.
  public getAllMeals(selectedDate: Date): Observable<Meal[]> {
    const dateUTC = selectedDate.toUTCString();
    return this.httpClient.get<Meal[]>(this.mealsCalendarUrl + '/' + dateUTC, this.httpOptions);
  }

  // Adds meal item (dish or product) to a meal.
  public addItemToMeal(request: AddMealItemRequest) {
    return this.httpClient.post<AddMealItemRequest>(
      `${this.mealsCalendarUrl}/add-meal-item`,
      request,
      this.httpOptions,
    );
  }
}
