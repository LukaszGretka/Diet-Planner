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

  constructor(private store: Store<GeneralState>, private dishStore: Store<DishState>, private router: Router) {}

  public ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    this.dishStore.dispatch(DishActions.loadDishesRequest());
  }

  public onEditButtonClick($event: any): void {
    this.router.navigate(['dishes/edit/' + ($event.target.parentElement as HTMLInputElement).value]);
  }
}
