import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import { Product } from 'src/app/products/models/product';
import * as GeneralActions from '../../stores/store.actions';
import { ProductService } from 'src/app/products/services/product.service';
import { ActivatedRoute } from '@angular/router';
import { map, Subscription } from 'rxjs';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css'],
})
export class EditProductComponent implements OnInit, OnDestroy {

  public product: Product;
  private routerSub: Subscription;

  constructor(private store: Store<GeneralState>, private productService: ProductService,
    private router: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.routerSub = this.router.params.subscribe(params => {
      this.productService.getProductById(params['id']).pipe(map(product => {
        this.product = product
      })).subscribe();
    });
  }

  ngOnDestroy(): void {
    this.routerSub.unsubscribe();
  }

  public editProductSubmit(): void {
    this.store.dispatch(GeneralActions.editProductRequest({
      productId: this.product.id, productData: this.product
    }));
  }
}
