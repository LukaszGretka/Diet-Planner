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
import { ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { GeneralReducer } from './stores/store.reducer';
import { EffectsModule } from '@ngrx/effects';
import { GeneralEffects } from './stores/store.effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { AddProductComponent } from './products/add-product/add-product.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    ProductsComponent,
    AddProductComponent,
    BodyProfileComponent,
    DashboardComponent,
    AddMeasurementComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    StoreModule.forRoot({
      generalState: GeneralReducer
    }
    ),
    EffectsModule.forRoot([
      GeneralEffects
    ]),
    StoreDevtoolsModule.instrument()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
