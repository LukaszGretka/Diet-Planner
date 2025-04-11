import { Component, inject } from '@angular/core';
import { map, take } from 'rxjs';
import { DishService } from '../services/dish.service';
import { ActivatedRoute } from '@angular/router';
import { Dish } from '../models/dish';
import { DishTemplateComponent } from '../dish-template/dish-template.component';

@Component({
  selector: 'app-dish-preview',
  templateUrl: './dish-preview.component.html',
  imports: [DishTemplateComponent],
})
export class DishPreviewComponent {
  private readonly dishService = inject(DishService);
  private readonly router = inject(ActivatedRoute);

  public dish: Dish;

  constructor() {
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
}
