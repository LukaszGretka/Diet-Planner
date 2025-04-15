import { Injectable, inject } from '@angular/core';
import { Product } from 'src/app/products/models/product';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private readonly httpClient = inject(HttpClient);

  private productsUrl = `${environment.dietPlannerApiUri}/api/product`;

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  getProductsWithPortion(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.productsUrl + '/all', { withCredentials: true });
  }

  getProductById(id: number): Observable<Product> {
    return this.httpClient.get<Product>(this.productsUrl + '/' + id, { withCredentials: true });
  }

  getProductByName(name: string): Observable<Product> {
    let params = new HttpParams();
    params = params.append('productName', name);
    return this.httpClient.get<Product>(this.productsUrl, { params: params, withCredentials: true });
  }

  addProduct(product: Product): Observable<Product> {
    debugger;
    return this.httpClient.post<Product>(this.productsUrl, product, this.httpOptions);
  }

  editProduct(productId: number, productData: Product): Observable<Product> {
    return this.httpClient.put<Product>(this.productsUrl + '/' + productId, productData, this.httpOptions);
  }

  removeProduct(productId: number): Observable<Product> {
    return this.httpClient.delete<Product>(this.productsUrl + '/' + productId, this.httpOptions);
  }
}
