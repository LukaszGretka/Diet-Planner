import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { SignUpRequest } from '../models/sign-up-request';
import * as AccountActions from '../stores/account.actions';
import { AccountState } from '../stores/account.state';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
})
export class SignUpComponent {
  constructor(
    private accountStore: Store<AccountState>,
    private notificationService: NotificationService
    ) { }

  public onSignUpSubmit(signUpRequest: SignUpRequest): void {
    var result = this.valiteSignUpInput(signUpRequest);

    if (result) {
      this.accountStore.dispatch(AccountActions.signUpRequest({ signUpRequest }));
    }
  }

  private valiteSignUpInput(signUpRequest: SignUpRequest): boolean {
    let validateResults = [ this.validateUserName(signUpRequest), this.validateEmailAddress(signUpRequest), this.validatePassword(signUpRequest)]
    if(validateResults.some(x => x === false)) {
      return false;
    }
    return true;
  }

  private validateUserName(signUpRequest: SignUpRequest): boolean {
    if (!signUpRequest.username) {
      this.notificationService.showErrorToast('Sign up error','User name cannot be empty.');
      return false;
    }
    else if (signUpRequest.username.length < 6 || signUpRequest.username.length >= 20) {
      this.notificationService.showErrorToast('Sign up error','User name should contains between 6 and 20 characters.');
      return false;
    }
    return true;
  }

  private validateEmailAddress(signUpRequest: SignUpRequest): boolean {
    if (!signUpRequest.email)
    {
      this.notificationService.showErrorToast('Sign up error','Email address should contains at least one character.');
      return false;
    }
    else if(!isNaN(Number(signUpRequest.email)))
    {
      this.notificationService.showErrorToast('Sign up error','Email contains invalid characters.');
      return false;
    }
    else if (!signUpRequest.email.match("[a-z0-9]+@[a-z]+\.[a-z]{2,3}")) 
    {
      this.notificationService.showErrorToast('Sign up error','Email format is invalid. Correct format is: example@example.com');
      return false;
    }
    return true;
  }

  private validatePassword(signUpRequest: SignUpRequest): boolean {
    if (signUpRequest.password.length < 8)
    {
      this.notificationService.showErrorToast('Sign up error', 'Password should contains at least eight characters.');
      return false;
    }
    return true;
  }
}
