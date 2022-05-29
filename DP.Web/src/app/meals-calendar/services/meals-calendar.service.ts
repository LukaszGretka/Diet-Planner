import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DailyMeals } from '../models/daily-meals';

@Injectable({
  providedIn: 'root'
})
export class MealsCalendarService {

  private mealsCalendarUrl = 'http://localhost:5000/api/mealsCalendar';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private httpClient: HttpClient) { }

  getDailyMeals(selectedDate: Date): Observable<DailyMeals> {
    return this.httpClient.get<DailyMeals>(this.mealsCalendarUrl + '/' + selectedDate.toISOString()); // TODO: check if date on backend
  }
}
