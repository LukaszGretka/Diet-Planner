import { Component, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import * as ProductActions from '../stores/products.actions';
import { Product } from 'src/app/products/models/product';
import { ProductTemplateComponent } from '../product-template/product-template.component';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css'],
  imports: [ProductTemplateComponent],
})
export class AddProductComponent {
  private readonly store = inject<Store<GeneralState>>(Store);

  public submitFunction: (store: any) => void;

  constructor() {
    this.submitFunction = this.getSubmitFunction();
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  submitForm(product: Product, returnUrl: string = '') {
    this.store.dispatch(ProductActions.addProductRequest({ productData: product, returnUrl }));
  }
}
