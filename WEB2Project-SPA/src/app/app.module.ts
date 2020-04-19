import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule } from 'ngx-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatCardModule} from '@angular/material/card';
import { MatDividerModule} from '@angular/material/divider';
import { MatFormFieldModule} from '@angular/material/form-field';
import { MatInputModule} from '@angular/material/input';
import { MatButtonModule} from '@angular/material/button';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AngularOpenlayersModule } from 'ngx-openlayers';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { ProfileComponent } from './profile/profile.component';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { CarrentalService } from './_services/carrental.service';
import { AuthService } from './_services/auth.service';
import { VehicleComponent } from './rentacar-profile/vehicle-card/vehicle.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSliderModule } from '@angular/material/slider';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDialogModule } from '@angular/material/dialog';
import { ReservationsComponent } from './reservations/reservations.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTabsModule } from '@angular/material/tabs';
import { RentalcompanyCardComponent } from './home/rentalcompany-card/rentalcompany-card.component';
import { AviocompanyCardComponent } from './home/aviocompany-card/aviocompany-card.component';
import { MatStepperModule } from '@angular/material/stepper';
import { ChartsModule } from 'ng2-charts';
import { AviocompanyProfileComponent } from './aviocompany-profile/aviocompany-profile.component';
import { FlightCardComponent } from './aviocompany-profile/flight-card/flight-card.component';
import { MatTableModule } from '@angular/material/table';
import { FieldsetModule } from 'primeng/fieldset';
import { AvioFlightsResolver } from './_resolvers/avio-flights-resolver';
import { PaginationModule } from 'ngx-bootstrap';
import { AvioProfileResolver } from './_resolvers/avio-profile-resolver';
import { AddVehicleDialogComponent } from './_dialogs/add-vehicle-dialog/add-vehicle-dialog.component';
import { CompanyIncomesDialogComponent } from './_dialogs/company-incomes-dialog/company-incomes-dialog.component';
import { CompanyReservationsDialogComponent } from './_dialogs/company-reservations-dialog/company-reservations-dialog.component';
import { EditAvioCompanyDialogComponent } from './_dialogs/edit-avio-company-dialog/edit-avio-company-dialog.component';
import { EditCarDialogComponent } from './_dialogs/edit-car-dialog/edit-car-dialog.component';
import { EditFlightDialogComponent } from './_dialogs/edit-flight-dialog/edit-flight-dialog.component';
import { EditrentalcompanydialogComponent } from './_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { RateVehicleDialogComponent } from './_dialogs/rate-vehicle-dialog/rate-vehicle-dialog.component';
import { ReservationDialogComponent } from './_dialogs/reservation-dialog/reservation-dialog.component';
import { SeatsDialogComponent } from './_dialogs/seats-dialog/seats-dialog.component';
import { SelectDatesDialogComponent } from './_dialogs/select-dates-dialog/select-dates-dialog.component';
import { ThankYouDialogComponent } from './_dialogs/thankYouDialog/thankYouDialog.component';
import { ThankYouForRateDialogComponent } from './_dialogs/thankYouForRateDialog/thankYouForRateDialog.component';
import { VehiclesOnDiscountDialogComponent } from './_dialogs/vehicles-on-discount-dialog/vehicles-on-discount-dialog.component';
import { ViewCarDealDialogComponent } from './_dialogs/viewCarDealDialog/viewCarDealDialog.component';
import { DiscountedVehicleComponent } from './rentacar-profile/discountedVehicle-card/discounted-vehicle.component';
import { ShowMapDialogComponent } from './_dialogs/show-map-dialog/show-map-dialog.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AddNewCompanyDialogComponent } from './_dialogs/add-new-company-dialog/add-new-company-dialog.component';
import { CompanyAddSuccessfullDialogComponent } from './_dialogs/company-add-successfull-dialog/company-add-successfull-dialog.component';
import { AddNewDestinationDialogComponent } from './_dialogs/add-new-destination-dialog/add-new-destination-dialog.component';
import { ChangeHeadofficeDialogComponent } from './_dialogs/change-headoffice-dialog/change-headoffice-dialog.component';
import { RemoveDestinationsDialogComponent } from './_dialogs/remove-destinations-dialog/remove-destinations-dialog.component';
import { reducers } from './app.reducer';
import { StoreModule } from '@ngrx/store';

export function tokenGetter() {
   return localStorage.getItem('token');
 }

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      ProfileComponent,
      RentalcompanyCardComponent,
      AviocompanyProfileComponent,
      FlightCardComponent,
      AviocompanyCardComponent,
      ReservationsComponent,
      VehicleComponent,
      RentacarProfileComponent,
      AddVehicleDialogComponent,
      CompanyIncomesDialogComponent,
      RemoveDestinationsDialogComponent,
      AddNewCompanyDialogComponent,
      AdminPanelComponent,
      CompanyReservationsDialogComponent,
      EditAvioCompanyDialogComponent,
      EditCarDialogComponent,
      CompanyAddSuccessfullDialogComponent,
      EditFlightDialogComponent,
      EditrentalcompanydialogComponent,
      RateVehicleDialogComponent,
      ReservationDialogComponent,
      AddNewDestinationDialogComponent,
      SeatsDialogComponent,
      SelectDatesDialogComponent,
      ChangeHeadofficeDialogComponent,
      ThankYouDialogComponent,
      ThankYouForRateDialogComponent,
      ShowMapDialogComponent,
      VehiclesOnDiscountDialogComponent,
      ViewCarDealDialogComponent,
      DiscountedVehicleComponent
   ],
   imports: [
      BrowserModule,
      OAuthModule.forRoot(),
      FormsModule,
      MatCheckboxModule,
      MatDialogModule,
      PaginationModule.forRoot(),
      FieldsetModule,
      MatAutocompleteModule,
      MatTabsModule,
      MatTableModule,
      MatStepperModule,
      BsDropdownModule,
      ChartsModule,
      MatSliderModule,
      MatPaginatorModule,
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
      VehiclesOnDiscountDialogComponent,
      ChangeHeadofficeDialogComponent,
      CompanyIncomesDialogComponent,
      AddNewDestinationDialogComponent,
      SelectDatesDialogComponent,
      CompanyReservationsDialogComponent,
      EditAvioCompanyDialogComponent,
      EditFlightDialogComponent,
      CompanyAddSuccessfullDialogComponent,
      ShowMapDialogComponent,
      RemoveDestinationsDialogComponent,
      AddNewCompanyDialogComponent,
      ReservationDialogComponent
   ]
})
export class AppModule { }