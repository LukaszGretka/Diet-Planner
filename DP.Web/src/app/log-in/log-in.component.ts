import {Component} from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import {GeneralState} from '../stores/store.state';
import {Store} from '@ngrx/store';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  constructor(private store: Store<GeneralState>) {}
  public logInSubmit(): void {
    console.log('signing in...');
  }
}
