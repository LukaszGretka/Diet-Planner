import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { SignInRequest } from '../models/sign-in-request';
import * as AccountActions from '../stores/account.actions';
import { AccountState } from '../stores/account.state';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css'],
})
export class SignInComponent implements OnInit {
  constructor(private accountStore: Store<AccountState>, private formBuilder: FormBuilder) {}
  public signInForm: FormGroup;

  ngOnInit(): void {
    this.signInForm = this.formBuilder.group({
      email: ['user@demo.com', { updateOn: 'blur', validators: [Validators.required, Validators.email] }],
      password: ['user@demo.com', { updateOn: 'blur', validators: [Validators.required] }],
    });
  }

  public onSubmit(): void {
    if (!this.signInForm.valid) {
      this.signInForm.markAllAsTouched();
      return;
    }

    this.accountStore.dispatch(
      AccountActions.signInRequest({
        signInRequest: {
          email: this.signInForm.get('email').value,
          password: this.signInForm.get('password').value,
        } as SignInRequest,
      }),
    );
  }
}
