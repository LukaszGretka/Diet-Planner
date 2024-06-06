import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { DashboardData } from "../models/dashboard-data";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private dashboardUrl = `${environment.dietPlannerApiUri}/api/dashboard`;

  public constructor(private httpClient: HttpClient) {

  }

  public getDashboardData(): Observable<DashboardData> {
    return this.httpClient.get<DashboardData>(this.dashboardUrl, { withCredentials: true })
  }
}
