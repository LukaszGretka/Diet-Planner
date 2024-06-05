import { Component, Input, OnInit } from "@angular/core";
import { ChartConfiguration } from "chart.js";
import { UtilityService } from "src/app/shared/services/utility.service";
import * as GeneralActions from '../../stores/store.actions';
import { Store } from '@ngrx/store';
import { GeneralState } from '../../stores/store.state';
import { DashboardStatsChartData } from "../models/dashboard-stats-chart-data";
import { Observable } from "rxjs";

@Component({
  selector: 'app-stats-canvas',
  templateUrl: './stats-canvas.component.html',
  styleUrls: ['./stats-canvas.component.css'],
})
export class StatsCanvasComponent implements OnInit {

  @Input()
  public dashboardChartData$: Observable<DashboardStatsChartData>;

  public selectedChartValue: number = 0;
  public mealCalendarSummaryChartData: ChartConfiguration['data'] = this.loadChartData(this.selectedChartValue);

  constructor(private store: Store<GeneralState>, private utilityService: UtilityService) {
    this.store.dispatch(GeneralActions.clearErrors());
  }

  public ngOnInit(): void {

  }

  public selectedChartValueChanged() {
    this.mealCalendarSummaryChartData = this.loadChartData(+this.selectedChartValue);
  }

  private loadChartData(statNumber: number): ChartConfiguration<'line'>['data'] {
    let data = [];
    const statToDisplay = +statNumber;
    switch (statToDisplay) {
      case 0:
        data = [{
          label: 'Calories',
          data: [2100, 2300, 2250, 1800, 3200, 2850, 1999],
          borderColor: 'rgba(112,218,157,0.7)',
          pointBackgroundColor: 'rgba(112,218,157,1)',
          backgroundColor: 'rgba(112,218,157,1)',
        }];
        break;
      case 1:
        data = [{
          label: 'Carbs',
          data: [100, 102, 150, 100, 76, 99, 57],
          borderColor: 'rgba(255,161,181,0.7)',
          pointBackgroundColor: 'rgba(255,161,181,1)',
          backgroundColor: 'rgba(255,161,181,1)',
        }];
        break;
      case 2:
        data = [{
          label: 'Proteins',
          data: [100, 102, 150, 100, 76, 99, 57],
          borderColor: 'rgba(134,199,243,0.7)',
          pointBackgroundColor: 'rgba(134,199,243,1)',
          backgroundColor: 'rgba(134,199,243,1)',
        }];
        break;
      case 3:
        data = [{
          label: 'Fats',
          data: [100, 102, 150, 100, 76, 99, 57],
          borderColor: 'rgba(255,226,154,0.7)',
          pointBackgroundColor: 'rgba(255,226,154,1)',
          backgroundColor: 'rgba(255,226,154,1)',
        }];
        break;
    }

    // For now no goals are implemented. Will be added later.
    const isGoalDefined = false;

    if (isGoalDefined) {
      data.push(this.getGoalData(statToDisplay));
    }

    return {
      labels: this.utilityService.getLast7Days(true),
      datasets: data
    };
  }

  // TODO: Complete it after implementation of Goals
  private getGoalData(statNumber: number) {
    let goalData = [];
    switch (statNumber) {
      case 0:
        goalData = ['2250', '2250', '2250', '2250', '2250', '2250', '2250']
        break;
      case 1:
        goalData = ['1']
        break;
      case 2:
        goalData = ['2']
        break;
      case 3:
        goalData = ['3']
        break;
    }

    return {
      label: 'Goal',
      type: 'line',
      data: goalData,
      backgroundColor: 'rgba(255,0,0,1)',
      borderColor: 'rgba(255,0,0,1)',
      pointBackgroundColor: 'rgba(255,0,0,1)',
    }
  }
}
