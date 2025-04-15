import { Component, OnInit, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { SignInRequest } from '../models/sign-in-request';
import * as AccountActions from '../stores/account.actions';
import * as AccountSelectors from '../stores/account.selector';
import { AccountState } from '../stores/account.state';
import { UntypedFormBuilder, UntypedFormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from '../../shared/form-error/form-error.component';
import { NgClass, AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css'],
  imports: [ReactiveFormsModule, FormErrorComponent, NgClass, RouterLink, AsyncPipe],
})
export class SignInComponent implements OnInit {
  private readonly accountStore = inject<Store<AccountState>>(Store);
  private readonly formBuilder = inject(UntypedFormBuilder);

  public signInForm: UntypedFormGroup;
  public isLoading$ = this.accountStore.select(AccountSelectors.isLoading);

  ngOnInit(): void {
    this.signInForm = this.formBuilder.group({
      username: ['', { validators: [Validators.required] }],
      password: ['', { validators: [Validators.required] }],
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
          username: this.signInForm.get('username').value,
          password: this.signInForm.get('password').value,
        } as SignInRequest,
      }),
    );
  }
}
