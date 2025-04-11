import { DecimalPipe } from '@angular/common';
import { Component, OnInit, PipeTransform } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { Store } from '@ngrx/store';
import { filter, map, Observable, startWith } from 'rxjs';
import { Product } from 'src/app/products/models/product';
import { GeneralState } from '../stores/store.state';
import * as ProductsActions from '../products/stores/products.actions';
import * as GeneralActions from '../stores/store.actions';
import * as ProductActions from '../products/stores/products.actions';
import { Router } from '@angular/router';
import * as StoreSelector from '../stores/store.selectors';
import * as ProductSelectors from './../products/stores/products.selectors';
import { AccountService } from '../account/services/account.service';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrls: ['./products.component.css'],
    providers: [DecimalPipe],
    standalone: false
})
export class ProductsComponent implements OnInit {
  public filter = new UntypedFormControl('');
  public filteredProducts$: Observable<Product[]>;

  public products$ = this.store.select(ProductSelectors.getAllProducts);
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public authenticatedUser$ = this.accountService.getUser();
  private productId: number;

  constructor(
    pipe: DecimalPipe,
    private store: Store<GeneralState>,
    private router: Router,
    private accountService: AccountService,
  ) {
    this.filteredProducts$ = this.filter.valueChanges.pipe(
      filter(x => x !== ''),
      startWith(''),
      map(text => search(text, pipe)),
    );
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

//TODO: Not working. Need to do something with that.
function search(text: string, pipe: PipeTransform): Product[] {
  return this.products$.filter(product => {
    const term = text.toLowerCase();
    return product.name.toLowerCase().includes(term);
  });
}
