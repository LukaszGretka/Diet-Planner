import { Component, inject } from '@angular/core';
import { DishState } from '../stores/dish.state';
import { Store } from '@ngrx/store';
import { Dish } from '../models/dish';
import * as DishActions from '../stores/dish.actions';
import { DishTemplateComponent } from '../dish-template/dish-template.component';

@Component({
  selector: 'app-dish-add',
  templateUrl: './dish-add.component.html',
  styleUrls: ['./dish-add.component.css'],
  imports: [DishTemplateComponent],
})
export class DishAddComponent {
  private readonly dishStore = inject<Store<DishState>>(Store);

  public submitFunction: (store: any) => void;

  constructor() {
    this.submitFunction = this.getSubmitFunction();
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  public submitForm(dish: Dish, returnUrl: string = ''): void {
    this.dishStore.dispatch(DishActions.saveDishRequest({ dish, returnUrl }));
  }
}
