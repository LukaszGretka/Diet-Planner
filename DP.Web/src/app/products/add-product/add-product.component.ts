import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import * as GeneralActions from '../../stores/store.actions';
import { Product } from 'src/models/product';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent {

  public product: Product = new Product();

  constructor(private store: Store<GeneralState>) { }

  public addProductSubmit(): void {
    this.store.dispatch(GeneralActions.addProductRequest({ productData: this.product }));
  }
}
