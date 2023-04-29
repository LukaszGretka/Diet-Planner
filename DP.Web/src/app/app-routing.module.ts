import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AddMeasurementComponent} from './body-profile/add-measurement/add-measurement.component';
import {BodyProfileComponent} from './body-profile/body-profile.component';
import {EditMeasurementComponent} from './body-profile/edit-measurement/edit-measurement.component';
import {DashboardComponent} from './dashboard/dashboard.component';
import {MealsCalendarComponent} from './meals-calendar/meals-calendar.component';
import {AddProductComponent} from './products/add-product/add-product.component';
import {EditProductComponent} from './products/edit-product/edit-product.component';
import {ProductsComponent} from './products/products.component';
import {SignInComponent} from './account/sign-in/sign-in.component';
import {SignUpComponent} from './account/sign-up/sign-up.component';
import {ErrorPageComponent} from './shared/error-page/error-page.component';
import {UnauthorizedComponent} from './account/unauthorized/unauthorized.component';
import {SignOutComponent} from './account/sign-out/sign-out.component';
import {AuthGuardService as AuthGuard} from './auth/auth-guard.service';
import {ToasterComponent} from './shared/toaster/toaster.component';

const routes: Routes = [
  {path: '', component: DashboardComponent, canActivate: [AuthGuard]},
  {path: 'error', component: ErrorPageComponent},
  {path: 'sign-in', component: SignInComponent},
  {path: 'sign-up', component: SignUpComponent},
  {path: 'unauthorized', component: UnauthorizedComponent},
  {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard]},
  {path: 'sign-out', component: SignOutComponent, canActivate: [AuthGuard]},
  {path: 'products', component: ProductsComponent, canActivate: [AuthGuard]},
  {path: 'products/edit/:id', component: EditProductComponent, canActivate: [AuthGuard]},
  {path: 'products/add', component: AddProductComponent, canActivate: [AuthGuard]},
  {path: 'body-profile', component: BodyProfileComponent, canActivate: [AuthGuard]},
  {path: 'body-profile/add', component: AddMeasurementComponent, canActivate: [AuthGuard]},
  {path: 'body-profile/edit/:id', component: EditMeasurementComponent, canActivate: [AuthGuard]},
  {path: 'meals-calendar', component: MealsCalendarComponent, canActivate: [AuthGuard]},
  {path: 'toast', component: ToasterComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
