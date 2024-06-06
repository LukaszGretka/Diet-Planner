import { DashboardData } from "../models/dashboard-data";

export interface DashboardState {
  dashboardData: DashboardData;
  errorCode: number | null;
}
