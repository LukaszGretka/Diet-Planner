import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductsComponent } from './products/products.component';
import { BodyProfileComponent } from './body-profile/body-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HttpClientModule } from '@angular/common/http';
import { AddMeasurementComponent } from './body-profile/add-measurement/add-measurement.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { GeneralReducer } from './stores/store.reducer';
import { EffectsModule } from '@ngrx/effects';
import { GeneralEffects } from './stores/store.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AddProductComponent } from './products/add-product/add-product.component';
import { ProductTemplateComponent } from './products/product-template/product-template.component';
import { EditProductComponent } from './products/edit-product/edit-product.component';
import { MeasurementTemplateComponent } from './body-profile/measurement-template/measurement-template.component';
import { EditMeasurementComponent } from './body-profile/edit-measurement/edit-measurement.component';
import { MealsCalendarComponent } from './meals-calendar/meals-calendar.component';
import { MealCalendarEffects } from './meals-calendar/stores/meals-calendar.effects';
import { SignInComponent } from './account/sign-in/sign-in.component';
import { SignUpComponent } from './account/sign-up/sign-up.component';
import { MealCalendarReducer } from './meals-calendar/stores/meals-calendar.reducer';
import { ErrorPageComponent } from './shared/error-page/error-page.component';
import { AccountEffects } from './account/stores/account.effects';
import { AccountService } from './account/services/account.service';
import { UnauthorizedComponent } from './account/unauthorized/unauthorized.component';
import { AuthGuardService } from './auth/auth-guard.service';
import { ToastComponent } from './shared/toast/toast.component';
import { AccountReducer } from './account/stores/account.reducer';
import { FormErrorComponent } from './shared/form-error/form-error.component';
import { MealCalendarTemplateComponent } from './meals-calendar/meal-calendar-template/meal-calendar-template.component';
import { NgChartsModule } from 'ng2-charts';
import { ProductsReducer } from './products/stores/products.reducer';
import { ProductsEffects } from './products/stores/products.effects';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    SignInComponent,
    SignUpComponent,
    ProductTemplateComponent,
    ProductsComponent,
    AddProductComponent,
    EditProductComponent,
    BodyProfileComponent,
    DashboardComponent,
    AddMeasurementComponent,
    MeasurementTemplateComponent,
    EditMeasurementComponent,
    MealsCalendarComponent,
    UnauthorizedComponent,
    ErrorPageComponent,
    ToastComponent,
    FormErrorComponent,
    MealCalendarTemplateComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    NgChartsModule,
    StoreModule.forRoot({
      generalState: GeneralReducer,
      productsState: ProductsReducer,
      mealCalendarState: MealCalendarReducer,
      accountState: AccountReducer,
    }),
    EffectsModule.forRoot([GeneralEffects, ProductsEffects, MealCalendarEffects, AccountEffects]),
    StoreDevtoolsModule.instrument(),
  ],
  providers: [AccountService, AuthGuardService],
  bootstrap: [AppComponent],
})
export class AppModule {}
