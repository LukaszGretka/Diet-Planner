import { Component, OnInit, inject } from '@angular/core';
import { Dish } from '../models/dish';
import { DishService } from '../services/dish.service';
import { map, take } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DishState } from '../stores/dish.state';
import { Store } from '@ngrx/store';
import * as DishActions from '../stores/dish.actions';
import * as DishSelectors from '../stores/dish.selectors';
import { DishTemplateComponent } from '../dish-template/dish-template.component';

@Component({
  selector: 'app-dish-edit',
  templateUrl: './dish-edit.component.html',
  styleUrls: ['./dish-edit.component.css'],
  imports: [DishTemplateComponent],
})
export class DishEditComponent {
  private readonly dishStore = inject<Store<DishState>>(Store);
  private readonly dishService = inject(DishService);
  private readonly router = inject(ActivatedRoute);

  public dish: Dish;
  public submitFunction: (store: any) => void;
  public currentDishes$ = this.dishStore.select(DishSelectors.getDishes);

  constructor() {
    this.submitFunction = this.getSubmitFunction();
    this.router.params.pipe(take(1)).subscribe(params => {
      this.dishService
        .getDishById(params['id'])
        .pipe(
          map(dish => {
            this.dish = dish;
          }),
        )
        .subscribe();
    });
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  private submitForm(dish: Dish, returnUrl: string = '') {
    this.dishStore.dispatch(DishActions.editDishRequest({ dish, returnUrl }));
  }
}
