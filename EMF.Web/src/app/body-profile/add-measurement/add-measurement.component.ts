import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Measurement } from 'src/models/measurement';
import * as BodyProfileActions from '../stores/body-profile.actions';
import { BodyProfileState } from '../stores/body-profile.state';
import * as BodyProfileSelectors from '../stores/body-profile.selectors'

@Component({
  selector: 'app-add-measurement',
  templateUrl: './add-measurement.component.html',
  styleUrls: ['./add-measurement.component.css']
})
export class AddMeasurementComponent {

  public addMeasurementError$ = this.store.select(BodyProfileSelectors.getAddMeasurementError);

  measurementForm = this.formBuilder.group({
    weight: '',
    chest: '',
    belly: '',
    waist: '',
    bicepsRight: '',
    bicepsLeft: '',
    forearmRight: '',
    forearmLeft: '',
    thighRight: '',
    thighLeft: '',
    calfRight: '',
    calfLeft: ''
  });

  constructor(private formBuilder: FormBuilder, private store: Store<BodyProfileState>) { }

  public measurementSubmit(): void {
    const measurementFormValue = this.measurementForm.value;

    const measurementData: Measurement = {
      weight: measurementFormValue.weight,
      chest: measurementFormValue.chest,
      belly: measurementFormValue.belly,
      waist: measurementFormValue.waist,
      bicepsRight: measurementFormValue.bicepsRight,
      bicepsLeft: measurementFormValue.bicepsLeft,
      forearmRight: measurementFormValue.forearmRight,
      forearmLeft: measurementFormValue.forearmLeft,
      thighRight: measurementFormValue.thighRight,
      thighLeft: measurementFormValue.thighLeft,
      calfRight: measurementFormValue.calfRight,
      calfLeft: measurementFormValue.calfLeft,
      date: new Date().toISOString()
    };

    this.store.dispatch(BodyProfileActions.setMeasurement({measurement: measurementData}));
    this.store.dispatch(BodyProfileActions.submitMeasurementRequest());
  }

}
