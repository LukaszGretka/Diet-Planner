import { Component, OnInit } from "@angular/core";
import { ChartConfiguration } from "chart.js";
import { UtilityService } from "src/app/shared/services/utility.service";
import * as GeneralActions from '../../stores/store.actions';
import { Store } from '@ngrx/store';
import { DashboardStatsChartData } from "../models/dashboard-stats-chart-data";
import { Observable } from "rxjs";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { DashboardState } from "../stores/dashboard.state";
import * as DashboardSelectors from "../stores/dashboard.selectors";

@UntilDestroy()
@Component({
  selector: 'app-stats-canvas',
  templateUrl: './stats-canvas.component.html',
  styleUrls: ['./stats-canvas.component.css'],
})
export class StatsCanvasComponent implements OnInit {

  public dashboardChartData$: Observable<DashboardStatsChartData>
    = this.store.select(DashboardSelectors.getDashboardData)

  public chartData: DashboardStatsChartData;
  public selectedChartValue: number = 0;
  public mealCalendarSummaryChartData: ChartConfiguration['data'];

  constructor(private store: Store<DashboardState>, private utilityService: UtilityService) {
    this.store.dispatch(GeneralActions.clearErrors());
  }

  public ngOnInit(): void {
    this.dashboardChartData$?.pipe(untilDestroyed(this)).subscribe(data => {
      if (!data) {
        return;
      }
      this.chartData = {
        caloriesLastSevenDays: data.caloriesLastSevenDays,
        carbsLastSevenDays: data.carbsLastSevenDays,
        proteinsLastSevenDays: data.proteinsLastSevenDays,
        fatsLastSevenDays: data.fatsLastSevenDays
      };
      this.mealCalendarSummaryChartData = this.loadStatSeparatedChartData(+this.selectedChartValue);
    })
  }

  public selectedChartValueChanged() {
    this.mealCalendarSummaryChartData = this.loadStatSeparatedChartData(+this.selectedChartValue);
  }

  private loadStatSeparatedChartData(statNumber: number): ChartConfiguration<'line'>['data'] {
    let data = [];
    const statToDisplay = +statNumber;
    switch (statToDisplay) {
      case 0:
        data = [{
          label: 'Calories',
          data: this.chartData.caloriesLastSevenDays,
          borderColor: 'rgba(112,218,157,0.7)',
          pointBackgroundColor: 'rgba(112,218,157,1)',
          backgroundColor: 'rgba(112,218,157,1)',
        }];
        break;
      case 1:
        data = [{
          label: 'Carbs',
          data: this.chartData.carbsLastSevenDays,
          borderColor: 'rgba(255,161,181,0.7)',
          pointBackgroundColor: 'rgba(255,161,181,1)',
          backgroundColor: 'rgba(255,161,181,1)',
        }];
        break;
      case 2:
        data = [{
          label: 'Proteins',
          data: this.chartData.proteinsLastSevenDays,
          borderColor: 'rgba(134,199,243,0.7)',
          pointBackgroundColor: 'rgba(134,199,243,1)',
          backgroundColor: 'rgba(134,199,243,1)',
        }];
        break;
      case 3:
        data = [{
          label: 'Fats',
          data: this.chartData.fatsLastSevenDays,
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
