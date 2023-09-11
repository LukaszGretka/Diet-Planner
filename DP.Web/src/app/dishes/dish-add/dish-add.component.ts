import { Component } from '@angular/core';
import { DishState } from '../stores/dish.state';
import { Store } from '@ngrx/store';
import { Dish } from '../models/dish';
import * as DishActions from '../stores/dish.actions';

@Component({
  selector: 'app-dish-add',
  templateUrl: './dish-add.component.html',
  styleUrls: ['./dish-add.component.css'],
})
export class DishAddComponent {
  public submitFunction: (store: any) => void;

  constructor(private dishStore: Store<DishState>) {
    this.submitFunction = this.getSubmitFunction();
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  public submitForm(dish: Dish, returnUrl: string = ''): void {
    this.dishStore.dispatch(DishActions.saveDishRequest({ dish, returnUrl }));
  }
}
