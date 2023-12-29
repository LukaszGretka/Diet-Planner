import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import * as GeneralActions from '../stores/store.actions';
import * as BodyProfileActions from './stores/body-profile.actions';
import * as GeneralSelector from '../stores/store.selectors';
import * as BodyProfileSelector from './stores/body-profile.selectors';
import { Router } from '@angular/router';
import { UntypedFormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-body-profile',
  templateUrl: './body-profile.component.html',
  styleUrls: ['./body-profile.component.css'],
})
export class BodyProfileComponent implements OnInit {
  public measurements$ = this.store.select(BodyProfileSelector.getMeasurements);
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  private processingMeasurementId: number;

  constructor(private store: Store<GeneralState>, private router: Router, private formBuilder: UntypedFormBuilder) {}

  public userProfileForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.maxLength(64)]],
    gender: ['', [Validators.maxLength(128)]],
    birthDate: [''],
    height: ['', [Validators.maxLength(3)]],
  });

  ngOnInit(): void {
    this.store.dispatch(GeneralActions.clearErrors());
    this.store.dispatch(BodyProfileActions.getMeasurementsRequest());
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
    this.store.dispatch(BodyProfileActions.removeMeasurementRequest({ measurementId: this.processingMeasurementId }));
  }

  public onSubmitUserProfile(): void {}
}
