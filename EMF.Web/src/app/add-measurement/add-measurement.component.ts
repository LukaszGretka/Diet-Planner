import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';

@Component({
  selector: 'app-add-measurement',
  templateUrl: './add-measurement.component.html',
  styleUrls: ['./add-measurement.component.css']
})
export class AddMeasurementComponent {

  public addMeasurementError = false;

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

  constructor(private formBuilder: FormBuilder, private measurementService: MeasurementService) { }

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

    this.measurementService.addMeasurement(measurementData).subscribe(measurement => {console.log(measurement)});
  }

}
