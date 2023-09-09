import { Component } from '@angular/core';
import { GeneralState } from '../stores/store.state';
import * as GeneralSelector from './../stores/store.selectors';
import * as GeneralActions from '../stores/store.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-dishes',
  templateUrl: './dishes.component.html',
  styleUrls: ['./dishes.component.css'],
})
export class DishesComponent {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  constructor(private store: Store<GeneralState>) {
    this.store.dispatch(GeneralActions.clearErrors());
  }
}
