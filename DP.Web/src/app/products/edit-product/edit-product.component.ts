import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import { Product } from 'src/app/products/models/product';
import * as ProductsActions from './../stores/products.actions';
import { ProductService } from 'src/app/products/services/product.service';
import { ActivatedRoute } from '@angular/router';
import { map, take } from 'rxjs';
import { ProductTemplateComponent } from '../product-template/product-template.component';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css'],
  imports: [ProductTemplateComponent],
})
export class EditProductComponent implements OnInit {
  private store = inject<Store<GeneralState>>(Store);
  private productService = inject(ProductService);
  private router = inject(ActivatedRoute);

  public product: Product;
  public submitFunction: (store: any) => void;

  constructor() {
    this.submitFunction = this.getSubmitFunction();
  }

  ngOnInit(): void {
    this.router.params.pipe(take(1)).subscribe(params => {
      this.productService
        .getProductById(params['id'])
        .pipe(
          map(product => {
            this.product = product;
          }),
        )
        .subscribe();
    });
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  private submitForm(product: Product) {
    this.store.dispatch(ProductsActions.editProductRequest({ productId: product.id, productData: product }));
  }
}
