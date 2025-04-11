import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { DashboardData } from '../models/dashboard-data';
import { Observable } from 'rxjs';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private readonly httpClient = inject(HttpClient);
  private readonly dashboardUrl = `${environment.dietPlannerApiUri}/api/dashboard`;

  public getDashboardData(): Observable<DashboardData> {
    return this.httpClient.get<DashboardData>(this.dashboardUrl, { withCredentials: true });
  }
}
