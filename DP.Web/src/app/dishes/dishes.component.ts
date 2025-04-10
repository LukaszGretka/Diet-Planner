import { Component, OnInit } from '@angular/core';
import { GeneralState } from '../stores/store.state';
import * as GeneralSelector from './../stores/store.selectors';
import * as GeneralActions from '../stores/store.actions';
import * as DishSelector from './stores/dish.selectors';
import { Store } from '@ngrx/store';
import { DishState } from './stores/dish.state';
import * as DishActions from './stores/dish.actions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dishes',
  templateUrl: './dishes.component.html',
  styleUrls: ['./dishes.component.css'],
})
export class DishesComponent implements OnInit {
  public dishes$ = this.dishStore.select(DishSelector.getDishes);
  public isLoading$ = this.dishStore.select(DishSelector.isLoading);

  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  private dishIdToRemove: number;

  constructor(private store: Store<GeneralState>, private dishStore: Store<DishState>, private router: Router) {}

  public ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    this.dishStore.dispatch(DishActions.loadDishesRequest());
  }

  public onEditButtonClick(dishId: number): void {
    this.router.navigate([('dish/edit/' + dishId) as string]);
  }

  public onPreviewButtonClick(dishId: number): void {
    this.router.navigate([('dish/preview/' + dishId) as string]);
  }

  public onDeleteButtonClick(dishId: number): void {
    this.dishIdToRemove = dishId;
  }

  public removeConfirmationButtonClick(): void {
    this.dishStore.dispatch(DishActions.deleteDishRequest({ id: this.dishIdToRemove }));
  }
}
