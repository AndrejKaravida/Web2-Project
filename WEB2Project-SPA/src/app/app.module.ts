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
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProfileComponent } from './profile/profile.component';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { CarrentalService } from './_services/carrental.service';
import { AuthService } from './_services/auth.service';
import { VehicleComponent } from './rentacar-profile/vehicle-card/vehicle.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { RentalcompanyCardComponent } from './home/rentalcompany-card/rentalcompany-card.component';
import { AviocompanyCardComponent } from './home/aviocompany-card/aviocompany-card.component';
import { ChartsModule } from 'ng2-charts';
import { AviocompanyProfileComponent } from './aviocompany-profile/aviocompany-profile.component';
import { FlightCardComponent } from './aviocompany-profile/flight-card/flight-card.component';
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
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { AddNewCompanyDialogComponent } from './_dialogs/add-new-company-dialog/add-new-company-dialog.component';
import { CompanyAddSuccessfullDialogComponent } from './_dialogs/company-add-successfull-dialog/company-add-successfull-dialog.component';
import { AddNewDestinationDialogComponent } from './_dialogs/add-new-destination-dialog/add-new-destination-dialog.component';
import { LayoutComponent } from './layout/layout.component';
import { UpdateUserprofileDialogComponent } from './_dialogs/update-userprofile-dialog/update-userprofile-dialog.component';
import { GraphicTicketDialogComponent } from './_dialogs/graphic-ticket-dialog/graphic-ticket-dialog.component';
import { IncomesAviocompanyDialogComponent } from './_dialogs/incomes-aviocompany-dialog/incomes-aviocompany-dialog.component';
import { DiscountFlightCardComponent } from './aviocompany-profile/discount-flight-card/discount-flight-card.component';
import { DiscountTicketListsComponent } from './aviocompany-profile/discount-ticket-lists/discount-ticket-lists.component';
import { ChangeHeadofficeDialogComponent } from './_dialogs/change-headoffice-dialog/change-headoffice-dialog.component';
import { RemoveDestinationsDialogComponent } from './_dialogs/remove-destinations-dialog/remove-destinations-dialog.component';
import { reducers } from './app.reducer';
import { StoreModule } from '@ngrx/store';
import { MaterialModule } from 'src/app/material.module';
import { DestinationsDialogComponent } from './_dialogs/destinations-dialog/destinations-dialog.component';
import { EditHeadofficeDialogComponent } from './_dialogs/edit-headoffice-dialog/edit-headoffice-dialog.component';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      ProfileComponent,
      RentalcompanyCardComponent,
      AviocompanyProfileComponent,
      FlightCardComponent,
      DiscountFlightCardComponent,
      DiscountTicketListsComponent,
      LayoutComponent,
      AviocompanyCardComponent,
      ReservationsComponent,
      VehicleComponent,
      RentacarProfileComponent,
      ViewCarDealDialogComponent,
      ReservationDialogComponent,
      EditrentalcompanydialogComponent,
      RateVehicleDialogComponent,
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
      DiscountedVehicleComponent,
      UpdateUserprofileDialogComponent,
      GraphicTicketDialogComponent,
      IncomesAviocompanyDialogComponent,
      DestinationsDialogComponent,
      EditHeadofficeDialogComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      MaterialModule,
      PaginationModule.forRoot(),
      ChartsModule,
      ReactiveFormsModule,
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
      ReservationDialogComponent,
      UpdateUserprofileDialogComponent,
      GraphicTicketDialogComponent,
      IncomesAviocompanyDialogComponent,
      DestinationsDialogComponent,
      EditHeadofficeDialogComponent
   ]
})
export class AppModule { }