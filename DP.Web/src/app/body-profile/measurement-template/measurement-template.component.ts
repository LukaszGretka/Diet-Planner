import { Component, OnInit, inject, input } from '@angular/core';
import { UntypedFormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Measurement } from 'src/app/body-profile/models/measurement';
import { FormErrorComponent } from '../../shared/form-error/form-error.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-measurement-template',
  templateUrl: './measurement-template.component.html',
  styleUrls: ['./measurement-template.component.css'],
  imports: [ReactiveFormsModule, FormErrorComponent, RouterLink],
})
export class MeasurementTemplateComponent implements OnInit {
  private formBuilder = inject(UntypedFormBuilder);

  public readonly measurement = input<Measurement>(undefined);

  public readonly submitFunction = input<Function>(undefined);

  private readonly defaultMeasurementValidator = [Validators.required, Validators.maxLength(3)];

  ngOnInit(): void {
    const measurement = this.measurement();
    if (measurement) {
      this.measurementForm.get('weight')?.setValue(measurement.weight);
      this.measurementForm.get('chest')?.setValue(measurement.chest);
      this.measurementForm.get('belly')?.setValue(measurement.belly);
      this.measurementForm.get('waist')?.setValue(measurement.waist);
      this.measurementForm.get('biceps-left')?.setValue(measurement.bicepsLeft);
      this.measurementForm.get('biceps-right')?.setValue(measurement.bicepsRight);
      this.measurementForm.get('forearm-left')?.setValue(measurement.forearmLeft);
      this.measurementForm.get('forearm-right')?.setValue(measurement.forearmRight);
      this.measurementForm.get('thigh-left')?.setValue(measurement.thighLeft);
      this.measurementForm.get('thigh-right')?.setValue(measurement.thighRight);
      this.measurementForm.get('calf-left')?.setValue(measurement.calfLeft);
      this.measurementForm.get('calf-right')?.setValue(measurement.calfRight);
    }
  }

  public measurementForm = this.formBuilder.group({
    weight: ['', this.defaultMeasurementValidator],
    chest: ['', this.defaultMeasurementValidator],
    belly: ['', this.defaultMeasurementValidator],
    waist: ['', this.defaultMeasurementValidator],
    'biceps-left': ['', this.defaultMeasurementValidator],
    'biceps-right': ['', this.defaultMeasurementValidator],
    'forearm-left': ['', this.defaultMeasurementValidator],
    'forearm-right': ['', this.defaultMeasurementValidator],
    'thigh-left': ['', this.defaultMeasurementValidator],
    'thigh-right': ['', this.defaultMeasurementValidator],
    'calf-left': ['', this.defaultMeasurementValidator],
    'calf-right': ['', this.defaultMeasurementValidator],
  });

  public onSubmit() {
    if (!this.measurementForm.valid) {
      this.measurementForm.markAllAsTouched();
      return;
    }
    this.submitFunction()({
      id: this.measurement()?.id,
      weight: this.getControlValue('weight'),
      chest: this.getControlValue('chest'),
      belly: this.getControlValue('belly'),
      waist: this.getControlValue('waist'),
      bicepsLeft: this.getControlValue('biceps-left'),
      bicepsRight: this.getControlValue('biceps-right'),
      forearmLeft: this.getControlValue('forearm-left'),
      forearmRight: this.getControlValue('forearm-right'),
      thighLeft: this.getControlValue('thigh-left'),
      thighRight: this.getControlValue('thigh-right'),
      calfLeft: this.getControlValue('calf-left'),
      calfRight: this.getControlValue('calf-right'),
    } as Measurement);
  }

  private getControlValue(controlName: string): any {
    return this.measurementForm.get(controlName)?.value;
  }
}
