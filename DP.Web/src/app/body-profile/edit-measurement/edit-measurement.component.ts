import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, Observable, Subscription } from 'rxjs';
import { GeneralState } from 'src/app/stores/store.state';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';
import * as GeneralActions from '../../stores/store.actions';
import * as StoreSelector from '../../stores/store.selectors'

@Component({
  selector: 'app-edit-measurement',
  templateUrl: './edit-measurement.component.html',
  styleUrls: ['./edit-measurement.component.css']
})
export class EditMeasurementComponent implements OnInit, OnDestroy {

  @Input()
  public measurement: Measurement = new Measurement();
  public editMeasurementError$ = this.store.select(StoreSelector.getError);

  private routerSub: Subscription;

  constructor(private store: Store<GeneralState>, private measurementService: MeasurementService,
    private router: ActivatedRoute) { }

  ngOnDestroy(): void {
    this.routerSub.unsubscribe();
  }

  ngOnInit(): void {
    this.routerSub = this.router.params.subscribe(params => {
      this.measurementService.getById(params['id']).pipe(map(measurement => {
        this.measurement = measurement
      })).subscribe();
    });
  }
  public measurementEdit(): void {
    this.store.dispatch(GeneralActions.submitEditMeasurementRequest({ measurement: this.measurement }));
  }

}
