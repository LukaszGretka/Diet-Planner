import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Measurement } from 'src/models/measurement';
import { MeasurementService } from 'src/services/measurement.service';

@Component({
  selector: 'app-body-profile',
  templateUrl: './body-profile.component.html',
  styleUrls: ['./body-profile.component.css']
})
export class BodyProfileComponent implements OnInit {

public measurements$ : Observable<Measurement[]> = new Observable<Measurement[]>();

  constructor(private measurementService: MeasurementService) { }

  async ngOnInit(): Promise<void> {
    this.measurements$ = await this.measurementService.getMeasurements();
  }

}
