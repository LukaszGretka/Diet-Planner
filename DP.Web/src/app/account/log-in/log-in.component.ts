import {Component, OnDestroy} from '@angular/core';
import {Store} from '@ngrx/store';
import {LogInRequest} from '../models/log-in-request';
import * as AccountActions from '../stores/account.actions';
import * as GeneralActions from '../../stores/store.actions';
import * as AccountSelector from '../stores/account.selector';
import * as GeneralSelector from './../../stores/store.selectors';
import {GeneralState} from './../../stores/store.state';
import {AccountState} from '../stores/account.state';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent {
  public user$ = this.accountStore.select(AccountSelector.getUser);

  constructor(private accountStore: Store<AccountState>) {}

  public onLogInSubmit(logInRequest: LogInRequest): void {
    this.accountStore.dispatch(AccountActions.logInRequest({logInRequest}));
  }
}
