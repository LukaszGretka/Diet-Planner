import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Dish } from '../models/dish';
import { environment } from 'src/environments/environment';
import { DishProduct } from '../models/dish-product';

@Injectable({
  providedIn: 'root',
})
export class DishService {
  private dishesUrl = `${environment.dietPlannerApiUri}/api/dish`;
  private dishProductUrl = `${environment.dietPlannerApiUri}/api/dishproduct`;

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

  public updatePortionMultiplier(dishId: number, productId: number, customizedPortionMultiplier: number) {
    return this.httpClient.patch(
      this.dishProductUrl,
      { dishId, productId, customizedPortionMultiplier },
      this.httpOptions,
    );
  }
}
