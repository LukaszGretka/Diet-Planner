import { DashboardStatsChartData } from "./dashboard-stats-chart-data";

export interface DashboardData extends DashboardStatsChartData {
  currentWeight: number;
}
