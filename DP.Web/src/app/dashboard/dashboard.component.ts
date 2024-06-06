import { Component, OnInit } from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import * as GeneralActions from '../stores/store.actions';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import { AccountService } from '../account/services/account.service';
import { DashboardService } from './services/dashboard.service';
import { BehaviorSubject, Observable } from 'rxjs';
import * as DashboardSelectors from "./stores/dashboard.selectors";
import { DashboardStatsChartData } from './models/dashboard-stats-chart-data';
import * as DashboardActions from "./stores/dashboard.actions";
import { DashboardData } from './models/dashboard-data';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public lastThreeDaysDateFormatted$ = new BehaviorSubject<string[]>([]);

  public dashboardChartData$: Observable<DashboardData> =
    this.store.select(DashboardSelectors.getDashboardData)

  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);

  public authenticatedUser$ = this.accountService.getUser();

  constructor(private store: Store<GeneralState>, private accountService: AccountService) {
    this.store.dispatch(GeneralActions.clearErrors());
  }

  public ngOnInit(): void {
    this.store.dispatch(DashboardActions.getDashboardDataRequest());
    this.setLast3Days();
  }

  private setLast3Days() {
    const result: string[] = [];
    const today = new Date();

    for (let i = 0; i < 3; i++) {
      const day = new Date(today);
      day.setDate(today.getDate() - i);
      const formattedDate = ('0' + day.getDate()).slice(-2) + '.' + ('0' + (day.getMonth() + 1)).slice(-2);
      result.push(formattedDate);
    }
    this.lastThreeDaysDateFormatted$.next(result);
  }
}
