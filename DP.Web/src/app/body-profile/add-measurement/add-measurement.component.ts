import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Measurement } from 'src/app/body-profile/models/measurement';
import * as BodyProfileActions from './../stores/body-profile.actions';
import { GeneralState } from '../../stores/store.state';
import * as StoreSelector from '../../stores/store.selectors';

@Component({
    selector: 'app-add-measurement',
    templateUrl: './add-measurement.component.html',
    styleUrls: ['./add-measurement.component.css'],
    standalone: false
})
export class AddMeasurementComponent {
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public submitFunction: (store: any) => void;

  constructor(private store: Store<GeneralState>) {
    this.submitFunction = this.getSubmitFunction();
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  submitForm(measurement: Measurement) {
    this.store.dispatch(BodyProfileActions.addMeasurementRequest({ measurementData: measurement }));
  }
}
