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
  take,
} from 'rxjs';
import * as ProductSelectors from './../../products/stores/products.selectors';
import * as DishSelectors from './../../dishes/stores/dish.selectors';
import { Store } from '@ngrx/store';
import { ProductsState } from 'src/app/products/stores/products.state';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { NotificationService } from 'src/app/shared/services/notification.service';
import * as ProductActions from './../../products/stores/products.actions';
import { DishProduct } from '../models/dish-product';
import { Product } from 'src/app/products/models/product';
import { ActivatedRoute, Router } from '@angular/router';
import { DishState } from '../stores/dish.state';

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

  public dishProducts$ = new BehaviorSubject<DishProduct[]>([]);
  public dishMacroSummary$: Observable<any>;
  public portionValue: number = 100;
  public searchItem: string = '';
  //TODO: taking list of products might be long (need to find better solution)
  public allProducts$ = this.productStore.select(ProductSelectors.getAllProducts);
  private returnUrl: string;
  private callbackMealProduct$ = this.dishStore.select(DishSelectors.getCallbackMealDish);

  constructor(
    private formBuilder: UntypedFormBuilder,
    private productStore: Store<ProductsState>,
    private dishStore: Store<DishState>,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.route.queryParams.subscribe(params => {
      if (params.hasOwnProperty('redirectUrl')) {
        this.callbackMealProduct$?.subscribe(item => {
          this.returnUrl = params['redirectUrl'];
          if (item) {
            this.dishForm.get('name').setValue(item.dishName);
          }
        });
      }
    });
  }

  public dishForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    description: ['', [Validators.maxLength(128)]],
    exposeToOtherUsers: [false],
  });

  ngOnInit(): void {
    this.productStore.dispatch(ProductActions.getAllProductsRequest());

    if (this.dish != null) {
      this.dishForm.get('name').setValue(this.dish.name);
      this.dishForm.get('description').setValue(this.dish.description);
      this.dishForm.get('exposeToOtherUsers').setValue(this.dish.exposeToOtherUsers);
      this.dishProducts$.next(this.dish.products);

      if(!this.dish.isOwner){
        this.dishForm.disable();
      }
    }
    this.dishMacroSummary$ = this.dishProducts$.pipe(
      map(product =>
        product.reduce(
          (total, dishProduct) => {
            (total.calories += dishProduct.product.calories),
              (total.carbohydrates += dishProduct.product.carbohydrates),
              (total.proteins += dishProduct.product.proteins),
              (total.fats += dishProduct.product.fats);
            return total;
          },
          {
            calories: 0,
            carbohydrates: 0,
            proteins: 0,
            fats: 0,
          },
        ),
      ),
    );
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
      id: this.dish != null ? this.dish.id : undefined,
    } as Dish, this.returnUrl);
  }

  public onBackButtonClick() {
    if (this.returnUrl) {
      this.router.navigate([this.returnUrl]);
    } else {
      this.router.navigate(['dishes']);
    }
  }

  public onPortionValueChange(poritonSize: number, productId: number) {
    if (poritonSize <= 0) {
      //TODO add validation error or auto replacing 0 or negative with value "1"
      return;
    }

    let dishProducts = this.dishProducts$.getValue();

    let dishProduct = dishProducts.find(dp => dp.product.id == productId);
    if (!dishProduct) {
      return;
    }

    let productToChange = { ...dishProduct.product } as Product;

    this.allProducts$
      .pipe(take(1))
      .pipe(map(products => products.find(p => p.id === productId)))
      .subscribe(originalProduct => {
        if (originalProduct) {
          dishProduct.portionMultiplier = poritonSize / 100;
          productToChange.calories = originalProduct.calories * dishProduct.portionMultiplier;
          productToChange.carbohydrates = originalProduct.carbohydrates * dishProduct.portionMultiplier;
          productToChange.proteins = originalProduct.proteins * dishProduct.portionMultiplier;
          productToChange.fats = originalProduct.fats * dishProduct.portionMultiplier;
        }
      });

    dishProduct.product = productToChange;

    this.dishProducts$.next(dishProducts);
  }

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
              portionMultiplier: 1, // Default is x1 which basically means 100g
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
