import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HomeComponent } from './home/home.component';
import { FormsModule } from '@angular/forms';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { CarrentalService } from './_services/carrental.service';
import { AuthService } from './_services/auth.service';
import { VehicleComponent } from './rentacar-profile/vehicle-card/vehicle.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { RentalcompanyCardComponent } from './home/rentalcompany-card/rentalcompany-card.component';
import { AviocompanyCardComponent } from './home/aviocompany-card/aviocompany-card.component';
import { AviocompanyProfileComponent } from './aviocompany-profile/aviocompany-profile.component';
import { FlightCardComponent } from './aviocompany-profile/flight-card/flight-card.component';
import { AvioFlightsResolver } from './_resolvers/avio-flights-resolver';
import { PaginationModule } from 'ngx-bootstrap';
import { AvioProfileResolver } from './_resolvers/avio-profile-resolver';
import { ShowMapDialogComponent } from './_dialogs/_rent_a_car_dialogs/show-map-dialog/show-map-dialog.component';
import { reducers } from './app.reducer';
import { StoreModule } from '@ngrx/store';
import { MaterialModule } from 'src/app/material.module';
import { AppDateAdapter, APP_DATE_FORMATS } from './format-datepicker';
import { AdminModule } from './admin-panel/admin.module';
import { PipesModule } from './pipes.module';
import { UserModule } from './user.module';
import { SearchFlightDialogComponent } from './_dialogs/_avio_company_dialogs/search-flight-dialog/search-flight-dialog.component';


@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RentalcompanyCardComponent,
      AviocompanyProfileComponent,
      FlightCardComponent,
      AviocompanyCardComponent,
      VehicleComponent,
      RentacarProfileComponent,
      ShowMapDialogComponent,
      SearchFlightDialogComponent
   ],
   imports: [
      BrowserModule,
      PipesModule,
      FormsModule,
      UserModule,
      AdminModule,
      MaterialModule,
      PaginationModule.forRoot(),
      BsDropdownModule.forRoot(),
      HttpClientModule,
      AppRoutingModule,
      BrowserAnimationsModule,
      FontAwesomeModule,
      StoreModule.forRoot(reducers)
   ],
   providers: [
      AuthService,
      CarrentalService,
      AvioProfileResolver,
      RentaCarProfileResolver,
      AvioFlightsResolver,
      VehicleListResolver,
      MatDatepickerModule,
      MatNativeDateModule,
      {provide: DateAdapter, useClass: AppDateAdapter},
      {provide: MAT_DATE_FORMATS, useValue: APP_DATE_FORMATS }
   ],
   bootstrap: [
      AppComponent
   ],
   entryComponents: [
      ShowMapDialogComponent,
      SearchFlightDialogComponent
   ]
})
export class AppModule { }