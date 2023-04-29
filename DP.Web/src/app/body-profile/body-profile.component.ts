import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import * as GeneralSelector from '../stores/store.selectors';
import { Router } from '@angular/router';

@Component({
  selector: 'app-body-profile',
  templateUrl: './body-profile.component.html',
  styleUrls: ['./body-profile.component.css'],
})
export class BodyProfileComponent implements OnInit {
  public measurements$ = this.store.select(GeneralSelector.getMeasurements);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  private processingMeasurementId: number;

  constructor(private store: Store<GeneralState>, private router: Router) {}

  ngOnInit(): void {
    this.store.dispatch(GeneralActions.getMeasurementsRequest());
  }

  onEditButtonClick($event: any): void {
    this.router.navigate(['body-profile/edit/' + ($event.target.parentElement as HTMLInputElement).value]);
  }

  onRemoveButtonClick($event: any): void {
    this.processingMeasurementId = Number(($event.target.parentElement as HTMLInputElement).value);
  }

  removeConfirmationButtonClick(): void {
    if (!this.processingMeasurementId) {
      return;
    }
    this.store.dispatch(GeneralActions.removeMeasurementRequest({ measurementId: this.processingMeasurementId }));
  }
}
