import { Component, Input } from '@angular/core';
import { Product } from 'src/app/products/models/product';

@Component({
  selector: 'app-product-template',
  templateUrl: './product-template.component.html',
  styleUrls: ['./product-template.component.css'],
})
export class ProductTemplateComponent {

  @Input()
  public product: Product;

  constructor() {
  }
}
