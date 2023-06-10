import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Measurement } from 'src/app/body-profile/models/measurement';

@Component({
  selector: 'app-measurement-template',
  templateUrl: './measurement-template.component.html',
  styleUrls: ['./measurement-template.component.css'],
})
export class MeasurementTemplateComponent implements OnInit {
  @Input()
  public measurement: Measurement;

  @Input()
  public submitFunction: Function;

  private defaultMeasurementValidator = [
    Validators.required,
    Validators.maxLength(3),
    Validators.min(1)
  ];

  constructor(private formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    if (this.measurement) {
      this.measurementForm.get('weight')?.setValue(this.measurement.weight);
      this.measurementForm.get('chest')?.setValue(this.measurement.chest);
      this.measurementForm.get('belly')?.setValue(this.measurement.belly);
      this.measurementForm.get('waist')?.setValue(this.measurement.waist);
      this.measurementForm.get('biceps-left')?.setValue(this.measurement.bicepsLeft);
      this.measurementForm.get('biceps-right')?.setValue(this.measurement.bicepsRight);
      this.measurementForm.get('forearm-left')?.setValue(this.measurement.forearmLeft);
      this.measurementForm.get('forearm-right')?.setValue(this.measurement.forearmRight);
      this.measurementForm.get('thigh-left')?.setValue(this.measurement.thighLeft);
      this.measurementForm.get('thigh-right')?.setValue(this.measurement.thighRight);
      this.measurementForm.get('calf-left')?.setValue(this.measurement.calfLeft);
      this.measurementForm.get('calf-right')?.setValue(this.measurement.calfRight);
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
  })

  public onSubmit() {
    if (!this.measurementForm.valid) {
      this.measurementForm.markAllAsTouched();
      return;
    }
    this.submitFunction({
      id: this.measurement.id,
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
      calfRight: this.getControlValue('calf-right')
    } as Measurement);
  }

  private getControlValue(controlName: string): any {
    return this.measurementForm.get(controlName)?.value;
  }
}
