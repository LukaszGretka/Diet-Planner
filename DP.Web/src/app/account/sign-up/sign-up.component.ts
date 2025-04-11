import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { SignUpRequest } from '../models/sign-up-request';
import * as AccountActions from '../stores/account.actions';
import * as AccountSelectors from '../stores/account.selector';
import { AccountState } from '../stores/account.state';
import { UntypedFormBuilder, UntypedFormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from '../../shared/form-error/form-error.component';
import { NgClass, AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css'],
  imports: [ReactiveFormsModule, FormErrorComponent, NgClass, RouterLink, AsyncPipe],
})
export class SignUpComponent implements OnInit {
  private readonly accountStore = inject<Store<AccountState>>(Store);
  private readonly formBuilder = inject(UntypedFormBuilder);

  public signUpForm: UntypedFormGroup;
  public isLoading$ = this.accountStore.select(AccountSelectors.isLoading);
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
