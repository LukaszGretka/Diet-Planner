import {Component} from '@angular/core';
import {Store} from '@ngrx/store';
import {SignUpRequest} from '../models/sign-up-request';
import * as AccountActions from '../stores/account.actions';
import {AccountState} from '../stores/account.state';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
})
export class SignUpComponent {
  constructor(private accountStore: Store<AccountState>) {}

  public onSignUpSubmit(signUpRequest: SignUpRequest): void {
    this.accountStore.dispatch(AccountActions.signUpRequest({signUpRequest}));
  }
}
