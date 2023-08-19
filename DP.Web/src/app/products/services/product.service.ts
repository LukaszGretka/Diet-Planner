import { Injectable } from '@angular/core';
import { PortionProduct, Product } from 'src/app/products/models/product';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private productsUrl = 'http://localhost:5000/api/product';

  httpOptions = {
    withCredentials: true,
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  constructor(private httpClient: HttpClient) {}

  getProductsWithPortion(): Observable<PortionProduct[]> {
    return this.httpClient.get<PortionProduct[]>(this.productsUrl + '/all', { withCredentials: true });
  }

  getProductById(id: number): Observable<PortionProduct> {
    return this.httpClient.get<PortionProduct>(this.productsUrl + '/' + id, { withCredentials: true });
  }

  getProductByName(name: string): Observable<PortionProduct> {
    let params = new HttpParams();
    params = params.append('productName', name);
    return this.httpClient.get<PortionProduct>(this.productsUrl, { params: params, withCredentials: true });
  }

  addProduct(product: Product): Observable<Product> {
    return this.httpClient.post<Product>(this.productsUrl, product, this.httpOptions);
  }

  editProduct(productId: number, productData: Product): Observable<Product> {
    return this.httpClient.put<Product>(this.productsUrl + '/' + productId, productData, this.httpOptions);
  }

  removeProduct(productId: number): Observable<Product> {
    return this.httpClient.delete<Product>(this.productsUrl + '/' + productId, this.httpOptions);
  }
}
