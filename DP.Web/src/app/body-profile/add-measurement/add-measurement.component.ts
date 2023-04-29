import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Measurement } from 'src/app/body-profile/models/measurement';
import * as GeneralActions from '../../stores/store.actions';
import { GeneralState } from '../../stores/store.state';
import * as StoreSelector from '../../stores/store.selectors';

@Component({
  selector: 'app-add-measurement',
  templateUrl: './add-measurement.component.html',
  styleUrls: ['./add-measurement.component.css'],
})
export class AddMeasurementComponent {
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public measurement: Measurement = new Measurement();

  constructor(private store: Store<GeneralState>) {}

  public measurementSubmit(): void {
    this.store.dispatch(GeneralActions.addMeasurementRequest({ measurementData: this.measurement }));
  }
}
