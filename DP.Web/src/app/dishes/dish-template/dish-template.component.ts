import { Component, Input, OnInit } from '@angular/core';
import { Dish } from '../models/dish';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  Observable,
  OperatorFunction,
  debounceTime,
  distinctUntilChanged,
  map,
  of,
  switchMap,
} from 'rxjs';
import * as ProductSelectors from './../../products/stores/products.selectors';
import { Store } from '@ngrx/store';
import { ProductsState } from 'src/app/products/stores/products.state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { PortionProduct, Product } from 'src/app/products/models/product';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { DishState } from '../stores/dish.state';
import * as DishActions from '../stores/dish.actions';
import * as ProductActions from './../../products/stores/products.actions';

@UntilDestroy()
@Component({
  selector: 'app-dish-template',
  templateUrl: './dish-template.component.html',
  styleUrls: ['./dish-template.component.css'],
})
export class DishTemplateComponent implements OnInit {
  @Input()
  public dish: Dish;

  @Input()
  public submitFunction: Function;

  public dishProducts$ = new BehaviorSubject<PortionProduct[]>([]);
  public portionValue = 100;
  public searchItem: string;
  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.productStore.select(ProductSelectors.getAllProducts);

  constructor(
    private formBuilder: UntypedFormBuilder,
    private dishStore: Store<DishState>,
    private productStore: Store<ProductsState>,
    private notificationService: NotificationService,
  ) {}

  ngOnInit(): void {
    this.productStore.dispatch(ProductActions.getAllProductsRequest());
  }

  public dishForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    description: ['', [Validators.maxLength(128)]],
  });

  public onSubmit(): void {
    if (!this.dishForm.valid) {
      this.dishForm.markAllAsTouched();
      return;
    }
    this.submitFunction(); //TODO add object to pass dish
  }

  public onPortionValueChange(poritonSize: number, behaviorSubject: BehaviorSubject<any>, index: number) {}

  public onRemoveProductButtonClick(behaviorSubject: BehaviorSubject<any>, index: number): void {}

  public onAddProductButtonClick(productName: string): void {
    if (productName) {
      this.allProducts$
        .pipe(map(products => products.find(p => p.name === productName)))
        .pipe(untilDestroyed(this))
        .subscribe(foundProduct => {
          if (foundProduct) {
            if (this.dishProducts$.getValue().filter(p => p.id == foundProduct.id).length > 0) {
              this.notificationService.showWarningToast(
                'Product already exist in this meal.',
                'Please edit portion box to adjust the entry.',
                5000,
              );
              return;
            }
            const products = (this.dishProducts$.getValue() as PortionProduct[]).concat(foundProduct as PortionProduct);
            this.dishProducts$.next(products);
            this.searchItem = '';
          } else {
            // TODO add functionality with adding modal later
            console.log('trying to add no existing product');
          }
        });
    }
  }

  public searchProducts: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(searchProduct => {
        if (searchProduct.length < 1) {
          return of([]);
        } else {
          return this.allProducts$.pipe(
            map(products =>
              products
                .filter(product => product.name.toLowerCase().includes(searchProduct.toLowerCase()))
                .slice(0, 10)
                .map(p => p.name),
            ),
          );
        }
      }),
    );
}
