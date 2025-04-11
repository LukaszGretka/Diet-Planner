import { Component } from '@angular/core';
import { AppModule } from '../../app.module';
import { map, take } from 'rxjs';
import { Store } from '@ngrx/store';
import { DishState } from '../stores/dish.state';
import { DishService } from '../services/dish.service';
import { ActivatedRoute } from '@angular/router';
import { Dish } from '../models/dish';
import { NgIf } from '@angular/common';

@Component({
    selector: 'app-dish-preview',
    templateUrl: './dish-preview.component.html',
    standalone: false
})
export class DishPreviewComponent {
  public dish: Dish;

  constructor(private dishStore: Store<DishState>, private dishService: DishService, private router: ActivatedRoute) {
    this.router.params.pipe(take(1)).subscribe(params => {
      this.dishService.getDishById(params['id']).pipe(
          map(dish => { this.dish = dish })).subscribe()
    });
  }
}
