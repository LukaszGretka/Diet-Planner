import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import * as GeneralSelector from '../stores/store.selectors';
import { Router } from '@angular/router';

@Component({
  selector: 'app-body-profile',
  templateUrl: './body-profile.component.html',
  styleUrls: ['./body-profile.component.css']
})
export class BodyProfileComponent implements OnInit {

  public measurements$: Observable<Measurement[]> = new Observable<Measurement[]>();
  public errors$ = this.store.select(GeneralSelector.getError);
  private processingMeasurementId: string;

  constructor(private measurementService: MeasurementService, private store: Store<GeneralState>,
    private router: Router) { }

  ngOnInit(): void {
    this.measurements$ = this.measurementService.getMeasurements();
  }

  onEditButtonClick($event: Event): void {
    this.router.navigate(['body-profile/edit/' + ($event.target as HTMLInputElement).value]);
  }

  onRemoveButtonClick($event: Event): void {
    this.processingMeasurementId = ($event.target as HTMLInputElement).value;
  }

  removeConfirmationButtonClick(): void {
    console.log(this.processingMeasurementId);
    if (this.processingMeasurementId) {
      this.store.dispatch(GeneralActions.setError({
        message: "An error occurred during editing measurement. Please try again later."
      }))
      return;
    }
    this.store.dispatch(GeneralActions.submitRemoveMeasurementRequest({ id: this.processingMeasurementId }));
  }
}
