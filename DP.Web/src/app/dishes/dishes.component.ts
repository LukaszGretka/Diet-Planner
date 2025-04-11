import { Component, OnInit, inject } from '@angular/core';
import { GeneralState } from '../stores/store.state';
import * as GeneralSelector from './../stores/store.selectors';
import * as GeneralActions from '../stores/store.actions';
import * as DishSelector from './stores/dish.selectors';
import { Store } from '@ngrx/store';
import { DishState } from './stores/dish.state';
import * as DishActions from './stores/dish.actions';
import { Router, RouterLink } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-dishes',
  templateUrl: './dishes.component.html',
  styleUrls: ['./dishes.component.css'],
  imports: [RouterLink, ReactiveFormsModule, FormsModule, NgbTooltip, AsyncPipe],
})
export class DishesComponent implements OnInit {
  private readonly store = inject<Store<GeneralState>>(Store);
  private readonly dishStore = inject<Store<DishState>>(Store);
  private readonly router = inject(Router);

  public dishes$ = this.dishStore.select(DishSelector.getDishes);
  public isLoading$ = this.dishStore.select(DishSelector.isLoading);

  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  private dishIdToRemove: number;

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
