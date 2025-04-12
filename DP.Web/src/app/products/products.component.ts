import { DecimalPipe, AsyncPipe } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { UntypedFormControl, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Product } from 'src/app/products/models/product';
import { GeneralState } from '../stores/store.state';
import * as ProductsActions from '../products/stores/products.actions';
import * as GeneralActions from '../stores/store.actions';
import * as ProductActions from '../products/stores/products.actions';
import { Router, RouterLink } from '@angular/router';
import * as StoreSelector from '../stores/store.selectors';
import * as ProductSelectors from './../products/stores/products.selectors';
import { AccountService } from '../account/services/account.service';
import { filterItems } from '../shared/helpers/base-item-filter';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  providers: [DecimalPipe],
  imports: [RouterLink, ReactiveFormsModule, FormsModule, AsyncPipe],
})
export class ProductsComponent implements OnInit {
  private store = inject<Store<GeneralState>>(Store);
  private router = inject(Router);
  private accountService = inject(AccountService);

  public filter = new UntypedFormControl('');
  public filteredProducts$: Observable<Product[]>;

  public products$ = this.store.select(ProductSelectors.getAllProducts);
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public authenticatedUser$ = this.accountService.getUser();
  private productId: number;

  public constructor() {
    this.filteredProducts$ = filterItems(this.products$, this.filter.valueChanges);
  }

  ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    this.store.dispatch(ProductActions.getAllProductsRequest());
  }

  onEditButtonClick($event: any): void {
    this.router.navigate(['product/edit/' + ($event.target.parentElement as HTMLInputElement).value]);
  }

  onOpenConfirmationModal($event: any) {
    this.productId = Number(($event.target.parentElement as HTMLInputElement).value);
  }

  removeConfirmationButtonClick(): void {
    this.store.dispatch(ProductsActions.removeProductRequest({ productId: this.productId }));
  }
}
