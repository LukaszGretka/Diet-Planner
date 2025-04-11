import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { MealItemRequest, Meal, UpdateMealItemPortionRequest } from '../models/meal';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MealsCalendarService {
  private readonly httpClient = inject(HttpClient);

  private mealsCalendarUrl = `${environment.dietPlannerApiUri}/api/mealsCalendar`;

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  // Gets all meals for selected date.
  public getAllMeals(selectedDate: Date): Observable<Meal[]> {
    const dateUTC = selectedDate.toUTCString();
    return this.httpClient.get<Meal[]>(this.mealsCalendarUrl + '/' + dateUTC, this.httpOptions);
  }

  // Adds meal item (dish or product) to a meal.
  public addItemToMeal(request: MealItemRequest) {
    return this.httpClient.post<MealItemRequest>(`${this.mealsCalendarUrl}/add-meal-item`, request, this.httpOptions);
  }

  public removeItemFromMeal(request: MealItemRequest) {
    return this.httpClient.delete<MealItemRequest>(`${this.mealsCalendarUrl}/remove-meal-item`, {
      ...this.httpOptions,
      body: request,
    });
  }

  public updateMealItemPortion(request: UpdateMealItemPortionRequest) {
    return this.httpClient.patch(`${this.mealsCalendarUrl}/update-meal-item-portion`, request, this.httpOptions);
  }
}
