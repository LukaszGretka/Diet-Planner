import { Component, input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-error',
  templateUrl: './form-error.component.html',
})
export class FormErrorComponent {
  readonly control = input<AbstractControl>(undefined);

  getErrorKeys(): string[] {
    return Object.keys(this.control().errors);
  }

  getControlName(control: AbstractControl): string | null {
    const formGroup = control.parent.controls;
    return Object.keys(formGroup).find(name => control === formGroup[name]) || null;
  }

  getErrorMessage(errorKey: string): string {
    const errorValue = this.control().getError(errorKey);
    let fieldName = this.getControlName(this.control());
    fieldName = fieldName[0].toUpperCase() + fieldName.slice(1); // Change first letter to capital.
    if (errorKey === 'required') {
      return `Field '${fieldName}' is required.`;
    } else if (errorKey === 'minlength') {
      return `Field '${fieldName}' should contain at least ${errorValue.requiredLength} characters.`;
    } else if (errorKey === 'maxlength') {
      return `Field '${fieldName}' cannot exceed  ${errorValue.requiredLength} characters.`;
    } else if (errorKey === 'pattern') {
      return `Field '${fieldName}' contains invalid characters.`;
    } else if (errorKey === 'min') {
      return `Value of field '${fieldName}' is too low.`;
    } else if (errorKey === 'email') {
      return `Value is not an email. Please provide valid email address.`;
    }

    return '';
  }
}
