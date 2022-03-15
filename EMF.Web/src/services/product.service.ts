import { Injectable } from '@angular/core';
import { Product } from 'src/models/product';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productsUrl = 'http://localhost:5000/api/product/'; 

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productsUrl + 'all');
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(this.productsUrl + id);
  }

  addProduct(product: Product): Observable<any> {
    return this.http.post(this.productsUrl, product);
  }

  editProduct(id: number): Observable<any> {
    return this.http.put(this.productsUrl, id);
  }

  removeProduct(id: number): Observable<any> {
    return this.http.delete(this.productsUrl + id);
  }
}
