import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatSelectModule } from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { AngularOpenlayersModule } from 'ngx-openlayers';

import { HomeComponent } from './home/home.component';
import { JwtModule } from '@auth0/angular-jwt';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { OAuthModule } from 'angular-oauth2-oidc';
import { ProfileComponent } from './profile/profile.component';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { CarrentalService } from './_services/carrental.service';
import { AuthService } from './_services/auth.service';
import { VehicleComponent } from './rentacar-profile/vehicle/vehicle.component';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatSliderModule} from '@angular/material/slider';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatDialogModule} from '@angular/material/dialog';
import { ReservationsComponent } from './reservations/reservations.component';
import { EditrentalcompanydialogComponent } from './_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { ViewCarDealDialogComponent } from './_dialogs/editrentalcompanydialog/viewCarDealDialog/viewCarDealDialog.component';
import { ThankYouDialogComponent } from './_dialogs/editrentalcompanydialog/thankYouDialog/thankYouDialog.component';
import {MatTabsModule} from '@angular/material/tabs';
import { RateVehicleDialogComponent } from './_dialogs/editrentalcompanydialog/rate-vehicle-dialog/rate-vehicle-dialog.component';
import { ThankYouForRateDialogComponent } from './_dialogs/editrentalcompanydialog/thankYouForRateDialog/thankYouForRateDialog.component';
import { RentalcompanyCardComponent } from './home/rentalcompany-card/rentalcompany-card.component';
import { AviocompanyCardComponent } from './home/aviocompany-card/aviocompany-card.component';
import { AddVehicleDialogComponent } from './_dialogs/editrentalcompanydialog/add-vehicle-dialog/add-vehicle-dialog.component';
import {MatStepperModule} from '@angular/material/stepper';
import { EditCarDialogComponent } from './_dialogs/editrentalcompanydialog/edit-car-dialog/edit-car-dialog.component';
import { RemoveCarDialogComponent } from './_dialogs/editrentalcompanydialog/remove-car-dialog/remove-car-dialog.component';



export function tokenGetter() {
   return localStorage.getItem('token');
 }

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      ThankYouForRateDialogComponent,
      HomeComponent,
      ProfileComponent,
      RentalcompanyCardComponent,
      AviocompanyCardComponent,
      ReservationsComponent,
      VehicleComponent,
      ViewCarDealDialogComponent,
      EditrentalcompanydialogComponent,
      RateVehicleDialogComponent,
      AddVehicleDialogComponent,
      EditCarDialogComponent,
      RemoveCarDialogComponent,
      ThankYouDialogComponent,
      RentacarProfileComponent
   ],
   imports: [
      BrowserModule,
      OAuthModule.forRoot(),
      FormsModule,
      MatCheckboxModule,
      MatDialogModule,
      MatAutocompleteModule,
      MatTabsModule,
      MatStepperModule,
      MatSliderModule,
      FontAwesomeModule,
      ReactiveFormsModule,
      AngularOpenlayersModule,
      MatProgressSpinnerModule,
      BsDropdownModule.forRoot(),
      HttpClientModule,
      MatSelectModule,
      MatDatepickerModule,
      MatNativeDateModule ,
      AppRoutingModule,
      BrowserAnimationsModule,
      MatCardModule,
      MatDividerModule,
      MatFormFieldModule,
      MatInputModule,
      MatButtonModule,
      FontAwesomeModule,
      JwtModule.forRoot({
         config: {
           tokenGetter,
           whitelistedDomains: ['localhost:5000'],
           blacklistedRoutes: ['localhost:5000/api/auth']
         }
       })
   ],
   providers: [
      CarrentalService, 
      AuthService,
      RentaCarProfileResolver,
      VehicleListResolver,
      MatDatepickerModule,
      MatNativeDateModule
   ] ,
   bootstrap: [
      AppComponent
   ],
   entryComponents: [
      EditrentalcompanydialogComponent,
      ViewCarDealDialogComponent,
      ThankYouDialogComponent,
      RateVehicleDialogComponent,
      ThankYouForRateDialogComponent,
      AddVehicleDialogComponent,
      EditCarDialogComponent,
      RemoveCarDialogComponent
   ]
})
export class AppModule { }
