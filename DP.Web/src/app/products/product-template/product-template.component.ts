import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Product } from 'src/app/products/models/product';
import * as ProductsSelector from './../stores/products.selectors';
import { ProductsState } from '../stores/products.state';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-template',
  templateUrl: './product-template.component.html',
  styleUrls: ['./product-template.component.css'],
})
export class ProductTemplateComponent implements OnInit {
  @Input()
  public product: Product;

  @Input()
  public submitFunction: Function;

  private returnUrl: string;
  private callbackMealProduct$ = this.store.select(ProductsSelector.getCallbackMealProduct);

  constructor(private formBuilder: FormBuilder, private store: Store<ProductsState>, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      if (params.hasOwnProperty('redirectUrl')) {
        this.callbackMealProduct$?.subscribe(item => {
          if (item) {
            this.returnUrl = params['redirectUrl'];
            this.productForm.get('name').setValue(item.productName);
          }
        });
      }
    });
  }

  ngOnInit(): void {
    if (this.product) {
      this.productForm.get('name')?.setValue(this.product.name);
      this.productForm.get('description')?.setValue(this.product.description);
      this.productForm.get('barcode')?.setValue(this.product.barCode);
      this.productForm.get('calories')?.setValue(this.product.calories);
      this.productForm.get('carbohydrates')?.setValue(this.product.carbohydrates);
      this.productForm.get('proteins')?.setValue(this.product.proteins);
      this.productForm.get('fats')?.setValue(this.product.fats);
    }
  }

  public productForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    description: ['', [Validators.maxLength(128)]],
    barcode: ['', [Validators.pattern('^[0-9]+$'), Validators.maxLength(20)]],
    calories: ['', [Validators.required, Validators.maxLength(5)]],
    carbohydrates: ['', [Validators.required, Validators.maxLength(5)]],
    proteins: ['', [Validators.required, Validators.maxLength(5)]],
    fats: ['', [Validators.required, Validators.maxLength(5)]],
  });

  public onSubmit() {
    if (!this.productForm.valid) {
      this.productForm.markAllAsTouched();
      return;
    }

    this.submitFunction(
      {
        id: this.product?.id,
        name: this.getControlValue('name'),
        description: this.getControlValue('description'),
        barCode: this.getControlValue('barcode'),
        calories: this.getControlValue('calories'),
        carbohydrates: this.getControlValue('carbohydrates'),
        proteins: this.getControlValue('proteins'),
        fats: this.getControlValue('fats'),
      } as Product,
      this.returnUrl,
    );
  }

  private getControlValue(controlName: string): any {
    return this.productForm.get(controlName)?.value;
  }
}
