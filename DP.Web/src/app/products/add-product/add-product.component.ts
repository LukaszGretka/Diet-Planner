import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import * as ProductActions from '../stores/products.actions';
import { Product } from 'src/app/products/models/product';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
})
export class AddProductComponent {
  public submitFunction: (store: any) => void;

  constructor(private store: Store<GeneralState>) {
    this.submitFunction = this.getSubmitFunction();
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  submitForm(product: Product, returnUrl: string = '') {
    this.store.dispatch(ProductActions.addProductRequest({ productData: product, returnUrl }));
  }
}
