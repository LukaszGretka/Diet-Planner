import { Component, Input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-form-error',
  template: `
    <small class="text-danger" *ngIf="control.invalid && control.touched">
      <ng-container *ngFor="let errorKey of getErrorKeys()">
        <div>{{ getErrorMessage(errorKey) }}</div>
      </ng-container>
    </small>
  `,
})
export class FormErrorComponent {
  @Input() control: AbstractControl;

  getErrorKeys(): string[] {
    return Object.keys(this.control.errors);
  }

  getControlName(control: AbstractControl): string | null {
    const formGroup = control.parent.controls;
    return Object.keys(formGroup).find(name => control === formGroup[name]) || null;
  }

  getErrorMessage(errorKey: string): string {
    const errorValue = this.control.getError(errorKey);
    let fieldName = this.getControlName(this.control);
    fieldName = fieldName[0].toUpperCase() + fieldName.slice(1); // Change first letter to capital.

    if (errorKey === 'required') {
      return `Field '${fieldName}' is required.`;
    } else if (errorKey === 'minlength') {
      return `Field '${fieldName}' should contain at least ${errorValue.requiredLength} characters.`;
    } else if (errorKey === 'maxlength') {
      return `Field '${fieldName}' cannot exceed  ${errorValue.requiredLength} characters.`;
    } else if (errorKey === 'pattern') {
      return `Field '${fieldName}' contains invalid characters.`;
    }
    return '';
  }
}
