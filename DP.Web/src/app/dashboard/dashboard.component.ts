import { Component, OnInit } from '@angular/core';
import * as GeneralSelector from '../stores/store.selectors';
import * as GeneralActions from '../stores/store.actions';
import { Store } from '@ngrx/store';
import { GeneralState } from '../stores/store.state';
import { AccountService } from '../account/services/account.service';
import { DashboardService } from './services/dashboard.service';
import { Observable } from 'rxjs';
import { DashboardData } from './models/dashboard-data';
import { DashboardStatsChartData } from './models/dashboard-stats-chart-data';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  public errorCode$ = this.store.select(GeneralSelector.getErrorCode);
  public authenticatedUser$ = this.accountService.getUser();
  public dashboardData$: Observable<DashboardData>;

  public dashboardChartData: Observable<DashboardStatsChartData>;

  constructor(private store: Store<GeneralState>, private accountService: AccountService,
    private dashboardService: DashboardService) {
    this.store.dispatch(GeneralActions.clearErrors());
  }

  public ngOnInit(): void {
    this.dashboardData$ = this.dashboardService.getDashboardData();
    this.dashboardData$.subscribe(x => console.log(x))
  }
}
