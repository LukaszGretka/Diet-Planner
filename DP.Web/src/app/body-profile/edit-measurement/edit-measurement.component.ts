import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, Subscription } from 'rxjs';
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
export class EditMeasurementComponent implements OnInit, OnDestroy {
  @Input()
  public measurement: Measurement = new Measurement();
  public errorCode$ = this.store.select(StoreSelector.getErrorCode);

  private routerSub: Subscription;

  constructor(
    private store: Store<GeneralState>,
    private measurementService: MeasurementService,
    private router: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.routerSub = this.router.params.subscribe(params => {
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

  ngOnDestroy(): void {
    this.routerSub.unsubscribe();
  }

  public measurementEdit(): void {
    this.store.dispatch(
      GeneralActions.editMeasurementRequest({
        measurementId: this.measurement.id,
        measurementData: this.measurement,
      }),
    );
  }
}
