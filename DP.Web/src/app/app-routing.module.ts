import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddMeasurementComponent } from './body-profile/add-measurement/add-measurement.component';
import { BodyProfileComponent } from './body-profile/body-profile.component';
import { EditMeasurementComponent } from './body-profile/edit-measurement/edit-measurement.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AddProductComponent } from './products/add-product/add-product.component';
import { EditProductComponent } from './products/edit-product/edit-product.component';
import { ProductsComponent } from './products/products.component';

const routes: Routes = [
  { path: '', component: DashboardComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'products/edit/:id', component: EditProductComponent },
  { path: 'products/add', component: AddProductComponent },
  { path: 'body-profile', component: BodyProfileComponent },
  { path: 'body-profile/add', component: AddMeasurementComponent },
  { path: 'body-profile/edit/:id', component: EditMeasurementComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
