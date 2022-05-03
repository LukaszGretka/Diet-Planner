import { DecimalPipe } from '@angular/common';
import { Component, OnInit, PipeTransform } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { filter, lastValueFrom, map, Observable, startWith } from 'rxjs';
import { Product } from 'src/models/product';
import { ProductService } from 'src/services/product.service';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  providers: [DecimalPipe]
})
export class ProductsComponent implements OnInit {

  public filter = new FormControl('');
  public products$: Observable<Product[]> = new Observable<Product[]>();

  constructor(private productService: ProductService, pipe: DecimalPipe, private store: Store<GeneralState>,
    private router: Router) {
    this.products$ = this.filter.valueChanges.pipe(
      startWith(''),
      map(text => search(text, pipe))
    );
  }

  ngOnInit(): void {
    this.products$ = this.productService.getProducts();
  }

  onEditButtonClick(event: Event): void {
    this.router.navigate(['products/edit/'+ (event.target as HTMLInputElement).value ]);
  }

  onRemoveButtonClick(event: Event): void {
    this.store.dispatch(GeneralActions.setProcessingProductId({ id:  (event.target as HTMLInputElement).value }));
  }

  removeConfirmationButtonClick(): void {
    this.store.dispatch(GeneralActions.submitRemoveProductRequest())
  }
}

//TODO: not working has to do something with that
function search(text: string, pipe: PipeTransform): Product[] {
  return this.products$.filter(product => {
    const term = text.toLowerCase();
    return product.name.toLowerCase().includes(term);
  });
}
