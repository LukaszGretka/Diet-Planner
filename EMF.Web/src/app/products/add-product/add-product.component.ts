import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';
import { GeneralState } from 'src/app/stores/store.state';
import * as GeneralActions from '../../stores/store.actions';
import { Product } from 'src/models/product';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  addProductForm = this.formBuilder.group({
    name: '',
    description: '',
    carbohydrates: '',
    proteins: '',
    fats: '',
    calories: '',
    barcode: ''
  });

  constructor(private formBuilder: FormBuilder, private store: Store<GeneralState>) { }

  ngOnInit(): void {
  }

  public addProductSubmit(): void {
    const productFormValue = this.addProductForm.value;

    const productData: Product = {
      name : productFormValue.name,
      barcode: productFormValue.barcode,
      calories: productFormValue.calories,
      carbohydrates : productFormValue.carbohydrates,
      proteins: productFormValue.proteins,
      fats: productFormValue.fats,
      description: productFormValue.description
    };

    this.store.dispatch(GeneralActions.setProduct({product: productData}));
    this.store.dispatch(GeneralActions.submitAddProductRequest());
  }
}
