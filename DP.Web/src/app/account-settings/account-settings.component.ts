import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormErrorComponent } from '../shared/form-error/form-error.component';
import { Store } from '@ngrx/store';
import { AccountState } from '../account/stores/account.state';
import * as AccountActions from '../account/stores/account.actions';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { pipe } from 'rxjs';

@UntilDestroy()
@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.css'],
  imports: [ReactiveFormsModule, FormErrorComponent],
})
export class AccountSettingsComponent implements OnInit {
  changePasswordForm: FormGroup;

  private readonly formBuilder = inject(FormBuilder);
  private readonly accountStore = inject(Store<AccountState>);

  ngOnInit(): void {
    this.changePasswordForm = this.formBuilder.group(
      {
        currentPassword: ['', { validators: [Validators.required] }],
        newPassword: ['', { validators: [Validators.required, Validators.minLength(6)] }],
        newPasswordConfirmed: ['', { validators: [Validators.required, Validators.minLength(6)] }],
      },
      { validators: this.passwordsMatchValidator },
    );
    this.accountStore
      .select(AccountActions.changePasswordRequestSuccess)
      .pipe(untilDestroyed(this))
      .subscribe(success => {
        if (success) {
          this.changePasswordForm.reset();
        }
      });
  }

  public onSubmitPasswordChange(): void {
    if (this.changePasswordForm.invalid || this.changePasswordForm.errors) {
      if (this.changePasswordForm.errors?.passwordMismatch) {
        this.changePasswordForm.get('newPasswordConfirmed').setErrors({ passwordMismatch: true });
      }

      this.changePasswordForm.markAllAsTouched();
      return;
    }
    this.accountStore.dispatch(
      AccountActions.changePasswordRequest({
        changePasswordRequest: {
          currentPassword: this.changePasswordForm.get('currentPassword').value,
          newPassword: this.changePasswordForm.get('newPassword').value,
          newPasswordConfirmed: this.changePasswordForm.get('newPasswordConfirmed').value,
        },
      }),
    );
  }

  passwordsMatchValidator = (group: FormGroup): { [key: string]: boolean } | null => {
    const newPassword = group.get('newPassword')?.value;
    const newPasswordConfirmed = group.get('newPasswordConfirmed')?.value;
    return newPassword === newPasswordConfirmed ? null : { passwordMismatch: true };
  };
}
