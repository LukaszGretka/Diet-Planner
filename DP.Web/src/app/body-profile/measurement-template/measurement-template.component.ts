import { Component, Input } from '@angular/core';
import { Measurement } from 'src/models/measurement';

@Component({
  selector: 'app-measurement-template',
  templateUrl: './measurement-template.component.html',
  styleUrls: ['./measurement-template.component.css'],
})
export class MeasurementTemplateComponent {
  @Input()
  public measurement: Measurement;

  constructor() {}
}
