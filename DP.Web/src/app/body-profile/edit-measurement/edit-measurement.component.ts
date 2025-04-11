import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs';
import { GeneralState } from 'src/app/stores/store.state';
import { Measurement } from 'src/app/body-profile/models/measurement';
import { MeasurementService } from 'src/app/body-profile/services/measurement.service';
import * as BodyProfileActions from './../stores/body-profile.actions';
import * as StoreSelector from '../../stores/store.selectors';
import { AsyncPipe } from '@angular/common';
import { MeasurementTemplateComponent } from '../measurement-template/measurement-template.component';

@Component({
  selector: 'app-edit-measurement',
  templateUrl: './edit-measurement.component.html',
  styleUrls: ['./edit-measurement.component.css'],
  imports: [MeasurementTemplateComponent, AsyncPipe],
})
export class EditMeasurementComponent implements OnInit {
  private readonly store = inject<Store<GeneralState>>(Store);
  private readonly measurementService = inject(MeasurementService);
  private readonly router = inject(ActivatedRoute);

  public measurement: Measurement;
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public submitFunction: (store: any) => void;

  constructor() {
    this.submitFunction = this.getSubmitFunction();
  }

  ngOnInit(): void {
    this.router.params.pipe(take(1)).subscribe(params => {
      this.measurementService
        .getById(params['id'])
        .pipe(
          map(measurement => {
            this.measurement = measurement;
          }),
        )
        .subscribe();
    });
  }

  private getSubmitFunction(): (formData: any) => void {
    return this.submitForm.bind(this);
  }

  private submitForm(measurement: Measurement) {
    this.store.dispatch(
      BodyProfileActions.editMeasurementRequest({
        measurementId: measurement.id,
        measurementData: measurement,
      }),
    );
  }
}
