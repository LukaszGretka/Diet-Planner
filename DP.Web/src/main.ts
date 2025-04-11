import { enableProdMode, importProvidersFrom } from '@angular/core';

import { environment } from './environments/environment';
import { AccountService } from './app/account/services/account.service';
import { AuthGuardService } from './app/auth/auth-guard.service';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { BrowserModule, bootstrapApplication } from '@angular/platform-browser';
import { AppRoutingModule } from './app/app-routing.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { ImageCropperModule } from 'ngx-image-cropper';
import { StoreModule } from '@ngrx/store';
import { GeneralReducer } from './app/stores/store.reducer';
import { BodyProfileReducer } from './app/body-profile/stores/body-profile.reducer';
import { ProductsReducer } from './app/products/stores/products.reducer';
import { MealCalendarReducer } from './app/meals-calendar/stores/meals-calendar.reducer';
import { AccountReducer } from './app/account/stores/account.reducer';
import { DishReducer } from './app/dishes/stores/dish.reducer';
import { DashboardReducer } from './app/dashboard/stores/dashboard.reducer';
import { EffectsModule } from '@ngrx/effects';
import { GeneralEffects } from './app/stores/store.effects';
import { BodyProfileEffects } from './app/body-profile/stores/body-profile.effects';
import { ProductsEffects } from './app/products/stores/products.effects';
import { MealCalendarEffects } from './app/meals-calendar/stores/meals-calendar.effects';
import { AccountEffects } from './app/account/stores/account.effects';
import { DishEffects } from './app/dishes/stores/dish.effects';
import { DashboardEffects } from './app/dashboard/stores/dashboard.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AppComponent } from './app/app.component';

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(
      BrowserModule,
      AppRoutingModule,
      NgbModule,
      ReactiveFormsModule,
      FormsModule,
      NgChartsModule,
      ImageCropperModule,
      StoreModule.forRoot({
        generalState: GeneralReducer,
        bodyProfileState: BodyProfileReducer,
        productsState: ProductsReducer,
        mealCalendarState: MealCalendarReducer,
        accountState: AccountReducer,
        dishState: DishReducer,
        dashboardState: DashboardReducer,
      }),
      EffectsModule.forRoot([
        GeneralEffects,
        BodyProfileEffects,
        ProductsEffects,
        MealCalendarEffects,
        AccountEffects,
        DishEffects,
        DashboardEffects,
      ]),
      StoreDevtoolsModule.instrument({ connectInZone: true }),
    ),
    AccountService,
    AuthGuardService,
    provideHttpClient(withInterceptorsFromDi()),
  ],
}).catch(err => console.error(err));
