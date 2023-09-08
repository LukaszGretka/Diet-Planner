import { Component } from '@angular/core';
import { GeneralState } from '../stores/store.state';
import * as GeneralSelector from './../stores/store.selectors';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-dishes',
  templateUrl: './dishes.component.html',
  styleUrls: ['./dishes.component.css']
})
export class DishesComponent {
  constructor(private store: Store<GeneralState>
  ) {

  }

  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

}
