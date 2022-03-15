import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';

@Component({
  selector: 'app-add-measurement',
  templateUrl: './add-measurement.component.html',
  styleUrls: ['./add-measurement.component.css']
})
export class AddMeasurementComponent implements OnInit {

  measurementForm = this.formBuilder.group({
    weight: '',
    chest: '',
    belly: '',
    waist: '',
    'biceps-left': '',
    'biceps-right': '',
    'forearm-left': '',
    'forearm-right': '',
    'thigh-left': '',
    'thigh-right': '',
    'calf-left': '',
    'calf-right': ''
  });

  constructor(private formBuilder: FormBuilder, private measurementService: MeasurementService) { }

  ngOnInit(): void {

  }

  public measurementSubmit(): void {
    const measurementFormValue = this.measurementForm.value;

    const measurementData: Measurement = {
      weight: measurementFormValue.weight,
      chest: measurementFormValue.chest,
      belly: measurementFormValue.belly,
      waist: measurementFormValue.waist,
      biceps: [ measurementFormValue['biceps-left'], measurementFormValue['biceps-right']],
      forearm : [measurementFormValue['forearm-left'], measurementFormValue['forearm-right']],
      thigh: [measurementFormValue['thigh-left'], measurementFormValue['thigh-right']],
      calf: [measurementFormValue['calf-left'], measurementFormValue['calf-right']]
    };

    this.measurementService.addMeasurement(measurementData).subscribe(measurement => {console.log(measurement)});
  }

}
