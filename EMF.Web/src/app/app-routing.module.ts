import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddMeasurementComponent } from './body-profile/add-measurement/add-measurement.component';
import { BodyProfileComponent } from './body-profile/body-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProductsComponent } from './products/products.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'body-profile', component: BodyProfileComponent },
  { path: 'body-profile/add', component: AddMeasurementComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
