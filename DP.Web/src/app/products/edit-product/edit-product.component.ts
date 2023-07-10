import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import { PortionProduct, Product } from 'src/app/products/models/product';
import * as GeneralActions from '../../stores/store.actions';
import { ProductService } from 'src/app/products/services/product.service';
import { ActivatedRoute } from '@angular/router';
import { map, take } from 'rxjs';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css'],
})
export class EditProductComponent implements OnInit {
  public product: Product;
  public submitFunction: (store: any) => void;

  constructor(
    private store: Store<GeneralState>,
    private productService: ProductService,
    private router: ActivatedRoute,
  ) {
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

  private submitForm(product: PortionProduct) {
    this.store.dispatch(GeneralActions.editProductRequest({ productId: product.id, productData: product }));
  }
}
