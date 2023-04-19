import {Component} from '@angular/core';
import {Store} from '@ngrx/store';
import {SignInRequest} from '../models/sign-in-request';
import * as AccountActions from '../stores/account.actions';
import * as AccountSelector from '../stores/account.selector';
import {AccountState} from '../stores/account.state';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css'],
})
export class SignInComponent {
  public authenticatedUser$ = this.accountStore.select(AccountSelector.getAuthenticatedUser);

  constructor(private accountStore: Store<AccountState>) {}

  public onSignInSubmit(signInRequest: SignInRequest): void {
    this.accountStore.dispatch(AccountActions.signInRequest({signInRequest}));
  }
}
