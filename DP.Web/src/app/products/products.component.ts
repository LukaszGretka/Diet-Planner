import {DecimalPipe} from '@angular/common';
import {Component, OnInit, PipeTransform} from '@angular/core';
import {FormControl} from '@angular/forms';
import {Store} from '@ngrx/store';
import {filter, map, Observable, startWith} from 'rxjs';
import {Product} from 'src/app/products/models/product';
import {GeneralState} from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import {Router} from '@angular/router';
import * as StoreSelector from '../stores/store.selectors';
import {AccountState} from '../account/stores/account.state';
import * as AccountSelector from '../account/stores/account.selector';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css'],
  providers: [DecimalPipe],
})
export class ProductsComponent implements OnInit {
  public filter = new FormControl('');
  public filteredProducts$: Observable<Product[]>;

  public products$ = this.store.select(StoreSelector.getProducts);
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public authenticatedUser$ = this.accountStore.select(AccountSelector.getAuthenticatedUser);
  private productId: number;

  constructor(
    pipe: DecimalPipe,
    private store: Store<GeneralState>,
    private router: Router,
    private accountStore: Store<AccountState>,
  ) {
    this.filteredProducts$ = this.filter.valueChanges.pipe(
      filter(x => x !== ''),
      startWith(''),
      map(text => search(text, pipe)),
    );
  }
  ngOnInit(): void {
    this.store.dispatch(GeneralActions.getProductsRequest());
  }

  onEditButtonClick($event: any): void {
    this.router.navigate(['products/edit/' + ($event.target.parentElement as HTMLInputElement).value]);
  }

  onOpenConfirmationModal($event: any) {
    this.productId = Number(($event.target.parentElement as HTMLInputElement).value);
  }

  removeConfirmationButtonClick(): void {
    this.store.dispatch(GeneralActions.removeProductRequest({productId: this.productId}));
  }
}

//TODO: not working has to do something with that
function search(text: string, pipe: PipeTransform): Product[] {
  return this.products$.filter(product => {
    const term = text.toLowerCase();
    return product.name.toLowerCase().includes(term);
  });
}
