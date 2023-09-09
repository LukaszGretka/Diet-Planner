import { Injectable } from '@angular/core';
import { Actions } from '@ngrx/effects';
import { DishService } from '../services/dish.service';

@Injectable()
export class DishEffects {
  constructor(private actions$: Actions, private dishService: DishService) {
    //...
  }
}
