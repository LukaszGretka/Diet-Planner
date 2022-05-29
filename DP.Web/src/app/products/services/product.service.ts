import { Injectable } from '@angular/core';
import { Product } from 'src/app/products/models/product';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productsUrl = 'http://localhost:5000/api/product';

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(private httpClient: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.httpClient.get<Product[]>(this.productsUrl + '/all');
  }

  getProductById(id: number): Observable<Product> {
    return this.httpClient.get<Product>(this.productsUrl + '/' + id);
  }

  addProduct(product: Product): Observable<Product> {
    return this.httpClient.post<Product>(this.productsUrl, product, this.httpOptions);
  }

  editProduct(productId: number, productData: Product): Observable<Product> {
    return this.httpClient.put<Product>(this.productsUrl + '/' + productId, productData, this.httpOptions);
  }

  removeProduct(productId: number): Observable<Product> {
    return this.httpClient.delete<Product>(this.productsUrl + "/" + productId, this.httpOptions);
  }
}
