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
import { NotificationService } from 'src/app/shared/services/notification.service';
import { DishState } from '../stores/dish.state';
import * as DishActions from '../stores/dish.actions';
import * as ProductActions from './../../products/stores/products.actions';
import { DishProduct } from '../models/dish-product';

@UntilDestroy()
@Component({
  selector: 'app-dish-template',
  templateUrl: './dish-template.component.html',
  styleUrls: ['./dish-template.component.css'],
})
export class DishTemplateComponent implements OnInit {
  @Input()
  public dishToEdit: Dish;

  @Input()
  public submitFunction: Function;

  public dishProducts$ = new BehaviorSubject<DishProduct[]>([]);
  public portionValue: number = 100;
  public searchItem: string = '';
  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.productStore.select(ProductSelectors.getAllProducts);

  constructor(
    private formBuilder: UntypedFormBuilder,
    private dishStore: Store<DishState>,
    private productStore: Store<ProductsState>,
    private notificationService: NotificationService,
  ) {}

  public dishForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    description: ['', [Validators.maxLength(128)]],
    exposeToOtherUsers: [false],
  });

  ngOnInit(): void {
    this.productStore.dispatch(ProductActions.getAllProductsRequest());

    if (this.dishToEdit != null) {
      this.dishForm.get('name').setValue(this.dishToEdit.name);
      this.dishForm.get('description').setValue(this.dishToEdit.description);
      this.dishForm.get('exposeToOtherUsers').setValue(this.dishToEdit.exposeToOtherUsers);

      this.dishProducts$.next(this.dishToEdit.products);
    }
  }

  public onSubmit(): void {
    if (!this.dishForm.valid) {
      this.dishForm.markAllAsTouched();
      return;
    }
    this.submitFunction({
      name: this.dishForm.get('name').value,
      description: this.dishForm.get('description').value,
      imagePath: this.dishForm.get('imagePath')?.value ?? '',
      products: this.dishProducts$.getValue(),
      exposeToOtherUsers: this.dishForm.get('exposeToOtherUsers').value,
      id: this.dishToEdit != null ? this.dishToEdit.id : undefined,
    } as Dish);
  }

  public onPortionValueChange(poritonSize: number, index: number) {}

  public onRemoveProductButtonClick(index: number): void {
    const products = this.dishProducts$.getValue();
    let productsLocal = [...products];
    productsLocal.splice(index, 1);
    this.dishProducts$.next(productsLocal);
  }

  public onAddProductButtonClick(productName: string): void {
    if (productName) {
      this.allProducts$
        .pipe(map(products => products.find(p => p.name === productName)))
        .pipe(untilDestroyed(this))
        .subscribe(foundProduct => {
          if (foundProduct) {
            if (
              this.dishProducts$.getValue().filter(dishProduct => dishProduct.product.id == foundProduct.id).length > 0
            ) {
              this.notificationService.showWarningToast(
                'Product already exist in this dish.',
                'Please edit portion box to adjust the entry.',
                5000,
              );
              return;
            }
            const products = this.dishProducts$.getValue().concat({
              product: foundProduct,
              portionMultiplier: 1,
            } as DishProduct);
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
