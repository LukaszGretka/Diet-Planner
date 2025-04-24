import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SignOutComponent } from './account/sign-out/sign-out.component';
import { AuthGuardService as AuthGuard } from './auth/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'error',
    loadComponent: () => import('./shared/error-page/error-page.component').then(m => m.ErrorPageComponent),
  },
  { path: 'sign-in', loadComponent: () => import('./account/sign-in/sign-in.component').then(m => m.SignInComponent) },
  { path: 'sign-up', loadComponent: () => import('./account/sign-up/sign-up.component').then(m => m.SignUpComponent) },
  {
    path: 'confirm-email-required',
    loadComponent: () =>
      import('./account/confirm-email-required/confirm-email-required.component').then(
        m => m.ConfirmEmailRequiredComponent,
      ),
  },
  {
    path: 'confirm-email',
    loadComponent: () => import('./account/confirm-email/confirm-email.component').then(m => m.ConfirmEmailComponent),
  },
  {
    path: 'unauthorized',
    loadComponent: () => import('./account/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent),
  },
  {
    path: 'dashboard',
    loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'account-settings',
    loadComponent: () => import('./account-settings/account-settings.component').then(m => m.AccountSettingsComponent),
    canActivate: [AuthGuard],
  },
  { path: 'sign-out', component: SignOutComponent, canActivate: [AuthGuard] },
  {
    path: 'products',
    loadComponent: () => import('./products/products.component').then(m => m.ProductsComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'product/edit/:id',
    loadComponent: () => import('./products/edit-product/edit-product.component').then(m => m.EditProductComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'products/add',
    loadComponent: () => import('./products/add-product/add-product.component').then(m => m.AddProductComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'body-profile',
    loadComponent: () => import('./body-profile/body-profile.component').then(m => m.BodyProfileComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'body-profile/add',
    loadComponent: () =>
      import('./body-profile/add-measurement/add-measurement.component').then(m => m.AddMeasurementComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'body-profile/edit/:id',
    loadComponent: () =>
      import('./body-profile/edit-measurement/edit-measurement.component').then(m => m.EditMeasurementComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'meals-calendar',
    loadComponent: () => import('./meals-calendar/meals-calendar.component').then(m => m.MealsCalendarComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'dishes',
    loadComponent: () => import('./dishes/dishes.component').then(m => m.DishesComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'dish/preview/:id',
    loadComponent: () => import('./dishes/dish-preview/dish-preview.component').then(m => m.DishPreviewComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'dishes/add',
    loadComponent: () => import('./dishes/dish-add/dish-add.component').then(m => m.DishAddComponent),
    canActivate: [AuthGuard],
  },
  {
    path: 'dish/edit/:id',
    loadComponent: () => import('./dishes/dish-edit/dish-edit.component').then(m => m.DishEditComponent),
    canActivate: [AuthGuard],
  },
  { path: 'toast', loadComponent: () => import('./shared/toast/toast.component').then(m => m.ToastComponent) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
