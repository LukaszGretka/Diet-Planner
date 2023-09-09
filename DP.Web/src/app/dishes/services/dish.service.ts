import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Dish } from '../models/dish';

@Injectable({
  providedIn: 'root',
})
export class DishService {
  private dishesUrl = 'http://localhost:5000/api/dish';

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  constructor(private httpClient: HttpClient) {}

  public getDishById(id: number): Observable<Dish> {
    return this.httpClient.get<Dish>(this.dishesUrl, { withCredentials: true });
  }

  public createDish(dish: Dish): Observable<Dish> {
    return this.httpClient.post<Dish>(this.dishesUrl, dish, this.httpOptions);
  }
}
