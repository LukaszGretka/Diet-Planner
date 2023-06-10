import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs';
import { GeneralState } from 'src/app/stores/store.state';
import { Measurement } from 'src/app/body-profile/models/measurement';
import { MeasurementService } from 'src/app/body-profile/services/measurement.service';
import * as GeneralActions from '../../stores/store.actions';
import * as StoreSelector from '../../stores/store.selectors';

@Component({
  selector: 'app-edit-measurement',
  templateUrl: './edit-measurement.component.html',
  styleUrls: ['./edit-measurement.component.css'],
})
export class EditMeasurementComponent implements OnInit {
  public measurement: Measurement;
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);
  public submitFunction: (store: any) => void;

  constructor(
    private store: Store<GeneralState>,
    private measurementService: MeasurementService,
    private router: ActivatedRoute,
  ) {
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
      GeneralActions.editMeasurementRequest({
        measurementId: measurement.id,
        measurementData: measurement,
      }),
    );
  }
}
