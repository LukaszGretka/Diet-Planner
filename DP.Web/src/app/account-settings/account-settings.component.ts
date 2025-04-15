import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormErrorComponent } from '../shared/form-error/form-error.component';
import { Store } from '@ngrx/store';
import { AccountState } from '../account/stores/account.state';
import * as AccountActions from '../account/stores/account.actions';

@Component({
  selector: 'app-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.css'],
  imports: [ReactiveFormsModule, FormErrorComponent],
})
export class AccountSettingsComponent {
  changePasswordForm: FormGroup;

  private readonly formBuilder = inject(FormBuilder);
  private readonly accountStore = inject(Store<AccountState>);

  constructor() {
    this.changePasswordForm = this.formBuilder.group(
      {
        currentPassword: ['', { updateOn: 'blur', validators: [Validators.required] }],
        newPassword: ['', { updateOn: 'blur', validators: [Validators.required] }],
        newPasswordConfirmed: ['', { updateOn: 'blur', validators: [Validators.required] }],
      },
      { validators: this.passwordsMatchValidator },
    );
  }

  public onSubmitPasswordChange(): void {
    if (this.changePasswordForm.invalid) {
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

  private passwordsMatchValidator(group: FormGroup): { [key: string]: string } | null {
    const newPassword = group.get('newPassword')?.value;
    const newPasswordConfirmed = group.get('newPasswordConfirmed')?.value;
    return newPassword === newPasswordConfirmed ? null : { error: 'passwordMismatch' };
  }
}
