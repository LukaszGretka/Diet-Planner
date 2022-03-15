import { Component, OnInit } from '@angular/core';
import { map, Observable, Subscription, switchMap } from 'rxjs';
import { Product } from 'src/models/product';
import { ProductService } from 'src/services/product.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  public products$: Observable<Product[]> = new Observable<Product[]>();

  constructor(private productService: ProductService) { 
  }

  ngOnInit(): void {
    this.products$ = this.productService.getProducts().pipe(x => x);
  }

}
