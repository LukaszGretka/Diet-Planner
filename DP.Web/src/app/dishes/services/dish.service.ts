import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Dish } from '../models/dish';
import { Product } from 'src/app/products/models/product';
import { DishProduct } from '../models/dish-product';

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

  public getUserDishes(): Observable<Dish[]> {
    return this.httpClient.get<Dish[]>(this.dishesUrl, { withCredentials: true });
  }

  public getDishById(id: number): Observable<Dish> {
    return this.httpClient.get<Dish>(this.dishesUrl + '/' + id, { withCredentials: true });
  }

  public saveDish(dish: Dish): Observable<Dish> {
    return this.httpClient.post<Dish>(this.dishesUrl, dish, this.httpOptions);
  }

  public editDish(dish: Dish): Observable<void> {
    return this.httpClient.patch<void>(this.dishesUrl, dish, this.httpOptions);
  }

  public deleteDish(id: number): Observable<void> {
    return this.httpClient.delete<void>(this.dishesUrl + '/' + id, { withCredentials: true });
  }

  public getDishProducts(dishId: number): Observable<DishProduct[]> {
    return this.httpClient.get<DishProduct[]>(this.dishesUrl + '/' + dishId + '/products', this.httpOptions);
  }

  public updatePortionMultiplier(dishId: number, productId: number, portionMultiplier: number) {
    return this.httpClient.patch(this.dishesUrl, { dishId, productId, portionMultiplier }, this.httpOptions);
  }
}
