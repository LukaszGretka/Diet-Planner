import { DecimalPipe } from '@angular/common';
import { Component, OnInit, PipeTransform } from '@angular/core';
import { FormControl } from '@angular/forms';
import { map, Observable, startWith } from 'rxjs';
import { Product } from 'src/models/product';
import { ProductService } from 'src/services/product.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  providers: [DecimalPipe]
})
export class ProductsComponent implements OnInit {

  filter = new FormControl('');

  constructor(private productService: ProductService, pipe: DecimalPipe) {
    this.products$ = this.filter.valueChanges.pipe(
      startWith(''),
      map(text => search(text, pipe))
    );
  }


  public products$: Observable<Product[]> = new Observable<Product[]>();

  ngOnInit(): void {
    this.products$ = this.productService.getProducts().pipe(x => x);
  }

}

function search(text: string, pipe: PipeTransform): Product[] {
  return this.products$.filter(product => {
    const term = text.toLowerCase();
    return product.name.toLowerCase().includes(term);
  });
}
