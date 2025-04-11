import { NgModule } from '@angular/core';
import annotationPlugin from 'chartjs-plugin-annotation';
import { Chart } from 'chart.js';

@NgModule()
export class AppModule {
  constructor() {
    Chart.register(annotationPlugin);
  }
}
