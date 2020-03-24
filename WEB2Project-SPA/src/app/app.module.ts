import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatSelectModule } from '@angular/material/select';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { AngularOpenlayersModule } from "ngx-openlayers";

import { HomeComponent } from './home/home.component';
import { JwtModule } from '@auth0/angular-jwt';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { OAuthModule } from 'angular-oauth2-oidc';
import { ProfileComponent } from './profile/profile.component';
import { DestinationCardComponent } from './destination-card/destination-card.component';
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


export function tokenGetter() {
   return localStorage.getItem('token');
 }

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      LoginComponent,
      RegisterComponent,
      HomeComponent,
      ProfileComponent,
      ReservationsComponent,
      VehicleComponent,
      DestinationCardComponent,
      EditrentalcompanydialogComponent,
      RentacarProfileComponent
   ],
   imports: [
      BrowserModule,
      OAuthModule.forRoot(),
      FormsModule,
      MatCheckboxModule,
      MatDialogModule,
      MatAutocompleteModule,
      MatSliderModule,
      FontAwesomeModule,
      ReactiveFormsModule,
      AngularOpenlayersModule,
      MatProgressSpinnerModule,
      BsDropdownModule.forRoot(),
      HttpClientModule,
      MatSelectModule,
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
   ] ,
   bootstrap: [
      AppComponent
   ],
   entryComponents: [
      EditrentalcompanydialogComponent
   ]
})
export class AppModule { }