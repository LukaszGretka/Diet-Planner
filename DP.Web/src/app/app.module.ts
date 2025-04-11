import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ProductsComponent } from './products/products.component';
import { BodyProfileComponent } from './body-profile/body-profile.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
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
import { MealSectionComponent } from './meals-calendar/components/meal-section/meal-section.component';
import { NgChartsModule } from 'ng2-charts';
import { ProductsReducer } from './products/stores/products.reducer';
import { ProductsEffects } from './products/stores/products.effects';
import { ConfirmEmailComponent } from './account/confirm-email/confirm-email.component';
import { ConfirmEmailRequiredComponent } from './account/confirm-email-required/confirm-email-required.component';
import { DishesComponent } from './dishes/dishes.component';
import { DishAddComponent } from './dishes/dish-add/dish-add.component';
import { DishTemplateComponent } from './dishes/dish-template/dish-template.component';
import { DishEditComponent } from './dishes/dish-edit/dish-edit.component';
import { DishEffects } from './dishes/stores/dish.effects';
import { DishReducer } from './dishes/stores/dish.reducer';
import { BodyProfileReducer } from './body-profile/stores/body-profile.reducer';
import { BodyProfileEffects } from './body-profile/stores/body-profile.effects';
import { ImageCropperModule } from 'ngx-image-cropper';
import annotationPlugin from 'chartjs-plugin-annotation';
import { Chart } from 'chart.js';
import { StatsCanvasComponent } from './dashboard/stats-canvas/stats-canvas.component';
import { DashboardEffects } from './dashboard/stores/dashboard.effects';
import { DashboardReducer } from './dashboard/stores/dashboard.reducer';
import { DishPreviewComponent } from './dishes/dish-preview/dish-preview.component';
import { DateSelectionComponent } from './meals-calendar/components/date-selection/date-selection.component';
import { MealItemSearchBarComponent } from './meals-calendar/components/meal-section/meal-item-search-bar/meal-item-search-bar.component';
import { MealCalendarChartComponent } from './meals-calendar/components/meal-calendar-chart/meal-calendar-chart.component';

@NgModule({ declarations: [
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
        DateSelectionComponent,
        UnauthorizedComponent,
        ErrorPageComponent,
        ToastComponent,
        FormErrorComponent,
        ConfirmEmailComponent,
        ConfirmEmailRequiredComponent,
        DishesComponent,
        DishTemplateComponent,
        DishAddComponent,
        DishEditComponent,
        DishPreviewComponent,
        StatsCanvasComponent,
        MealItemSearchBarComponent,
        MealCalendarChartComponent,
    ],
    bootstrap: [AppComponent],
    exports: [DishTemplateComponent], imports: [BrowserModule,
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
        MealSectionComponent], providers: [AccountService, AuthGuardService, provideHttpClient(withInterceptorsFromDi())] })
export class AppModule {
  constructor() {
    Chart.register(annotationPlugin);
  }
}
