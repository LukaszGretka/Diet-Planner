import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { LogInRequest } from '../models/log-in-request';
import * as AccountActions from '../stores/account.actions';
import * as AccountSelector from '../stores/account.selector';
import { AccountState } from '../stores/account.state';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css'],
})
export class LogInComponent {
  public authenticatedUser$ = this.accountStore.select(AccountSelector.getAuthenticatedUser);

  constructor(private accountStore: Store<AccountState>) { }

  public onLogInSubmit(logInRequest: LogInRequest): void {
    this.accountStore.dispatch(AccountActions.logInRequest({ logInRequest }));
  }
}
