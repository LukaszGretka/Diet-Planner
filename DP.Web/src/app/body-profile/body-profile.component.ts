import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';

@Component({
  selector: 'app-body-profile',
  templateUrl: './body-profile.component.html',
  styleUrls: ['./body-profile.component.css']
})
export class BodyProfileComponent implements OnInit {

public measurements$ : Observable<Measurement[]> = new Observable<Measurement[]>();

  constructor(private measurementService: MeasurementService, private store: Store<GeneralState>) { }

  async ngOnInit(): Promise<void> {
    this.measurements$ = await this.measurementService.getMeasurements();
  }

    removeConfirmationButtonClick(): void {
    this.store.dispatch(GeneralActions.submitRemoveMeasurementRequest());
  }

}
