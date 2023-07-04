import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SignUpRequest } from '../models/sign-up-request';
import * as AccountActions from '../stores/account.actions';
import { AccountState } from '../stores/account.state';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
})
export class SignUpComponent implements OnInit {
  public signUpForm: UntypedFormGroup;

  constructor(private accountStore: Store<AccountState>, private formBuilder: UntypedFormBuilder) {}
  ngOnInit(): void {
    this.signUpForm = this.formBuilder.group({
      email: ['', { updateOn: 'blur', validators: [Validators.required, Validators.email] }],
      username: [
        '',
        { updateOn: 'blur', validators: [Validators.required, Validators.minLength(6), Validators.maxLength(20)] },
      ],
      password: ['', { updateOn: 'blur', validators: [Validators.required, Validators.minLength(8)] }],
    });
  }

  public onSignUpSubmit(): void {
    if (!this.signUpForm.valid) {
      this.signUpForm.markAllAsTouched();
      return;
    }

    this.accountStore.dispatch(
      AccountActions.signUpRequest({
        signUpRequest: {
          username: this.signUpForm.get('username').value,
          email: this.signUpForm.get('email').value,
          password: this.signUpForm.get('password').value,
        } as SignUpRequest,
      }),
    );
  }
}
