// tslint:disable: max-line-length
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ViewCarDealDialogComponent } from './_dialogs/_rent_a_car_dialogs/viewCarDealDialog/viewCarDealDialog.component';
import { ReservationDialogComponent } from './_dialogs/_avio_company_dialogs/reservation-dialog/reservation-dialog.component';
import { EditrentalcompanydialogComponent } from './_dialogs/_rent_a_car_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { RateVehicleDialogComponent } from './_dialogs/_rent_a_car_dialogs/rate-vehicle-dialog/rate-vehicle-dialog.component';
import { AddVehicleDialogComponent } from './_dialogs/_rent_a_car_dialogs/add-vehicle-dialog/add-vehicle-dialog.component';
import { CompanyIncomesDialogComponent } from './_dialogs/_rent_a_car_dialogs/company-incomes-dialog/company-incomes-dialog.component';
import { RemoveDestinationsDialogComponent } from './_dialogs/_rent_a_car_dialogs/remove-destinations-dialog/remove-destinations-dialog.component';
import { ChangeVehicleLocationDialogComponent } from './_dialogs/_rent_a_car_dialogs/changeVehicleLocationDialog/changeVehicleLocationDialog.component';
import { CompanyReservationsDialogComponent } from './_dialogs/_rent_a_car_dialogs/company-reservations-dialog/company-reservations-dialog.component';
import { EditAvioCompanyDialogComponent } from './_dialogs/_avio_company_dialogs/edit-avio-company-dialog/edit-avio-company-dialog.component';
import { EditCarDialogComponent } from './_dialogs/_rent_a_car_dialogs/edit-car-dialog/edit-car-dialog.component';
import { CompanyAddSuccessfullDialogComponent } from './_dialogs/_adminpanel_dialogs/company-add-successfull-dialog/company-add-successfull-dialog.component';
import { EditFlightDialogComponent } from './_dialogs/_avio_company_dialogs/edit-flight-dialog/edit-flight-dialog.component';
import { SelectDatesDialogComponent } from './_dialogs/_rent_a_car_dialogs/select-dates-dialog/select-dates-dialog.component';
import { ChangeHeadofficeDialogComponent } from './_dialogs/_rent_a_car_dialogs/change-headoffice-dialog/change-headoffice-dialog.component';
import { ThankYouDialogComponent } from './_dialogs/_rent_a_car_dialogs/thankYouDialog/thankYouDialog.component';
import { ThankYouForRateDialogComponent } from './_dialogs/_rent_a_car_dialogs/thankYouForRateDialog/thankYouForRateDialog.component';
import { VehiclesOnDiscountDialogComponent } from './_dialogs/_rent_a_car_dialogs/vehicles-on-discount-dialog/vehicles-on-discount-dialog.component';
import { DiscountedVehicleComponent } from './rentacar-profile/discountedVehicle-card/discounted-vehicle.component';
import { GraphicTicketDialogComponent } from './_dialogs/_avio_company_dialogs/graphic-ticket-dialog/graphic-ticket-dialog.component';
import { IncomesAviocompanyDialogComponent } from './_dialogs/_avio_company_dialogs/incomes-aviocompany-dialog/incomes-aviocompany-dialog.component';
import { DestinationsDialogComponent } from './_dialogs/_avio_company_dialogs/destinations-dialog/destinations-dialog.component';
import { AddNewBranchDialogComponent } from './_dialogs/_rent_a_car_dialogs/add-new-branch-dialog/add-new-branch-dialog.component';
import { EditHeadofficeDialogComponent } from './_dialogs/_avio_company_dialogs/edit-headoffice-dialog/edit-headoffice-dialog.component';
import { ChartsModule } from 'ng2-charts';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { RateflightdialogComponent } from './_dialogs/_avio_company_dialogs/rateflightdialog/rateflightdialog.component';
import { RentacaroptiondialogComponent } from './_dialogs/_avio_company_dialogs/rentacaroptiondialog/rentacaroptiondialog.component';
import { DiscountedVehicleChooseDialogComponent } from './_dialogs/_rent_a_car_dialogs/discounted-vehicle-choose-dialog/discounted-vehicle-choose-dialog.component';
import { DiscountedVehicleDealsDialogComponent } from './_dialogs/_rent_a_car_dialogs/discounted-vehicle-deals-dialog/discounted-vehicle-deals-dialog.component';
import { BookDiscountedVehicleDialogComponent } from './_dialogs/_rent_a_car_dialogs/book-discounted-vehicle-dialog/book-discounted-vehicle-dialog.component';
import { HowManyDaysDialogComponent } from './_dialogs/_rent_a_car_dialogs/how-many-days-dialog/how-many-days-dialog.component';


@NgModule({
  declarations: [
    ViewCarDealDialogComponent,
    ReservationDialogComponent,
    EditrentalcompanydialogComponent,
    RateVehicleDialogComponent,
    AddVehicleDialogComponent,
    CompanyIncomesDialogComponent,
    RemoveDestinationsDialogComponent,
    ChangeVehicleLocationDialogComponent,
    CompanyReservationsDialogComponent,
    EditAvioCompanyDialogComponent,
    EditCarDialogComponent,
    CompanyAddSuccessfullDialogComponent,
    EditFlightDialogComponent,
    EditrentalcompanydialogComponent,
    RateVehicleDialogComponent,
    ReservationDialogComponent,
    SelectDatesDialogComponent,
    RentacaroptiondialogComponent,
    ChangeHeadofficeDialogComponent,
    ThankYouDialogComponent,
    ThankYouForRateDialogComponent,
    VehiclesOnDiscountDialogComponent,
    DiscountedVehicleComponent,
    GraphicTicketDialogComponent,
    IncomesAviocompanyDialogComponent,
    DestinationsDialogComponent,
    AddNewBranchDialogComponent,
    EditHeadofficeDialogComponent,
    RateflightdialogComponent,
    DiscountedVehicleChooseDialogComponent,
    DiscountedVehicleDealsDialogComponent,
    BookDiscountedVehicleDialogComponent,
    HowManyDaysDialogComponent
  ],
  imports: [
    CommonModule,
    ChartsModule,
    MaterialModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    FormsModule
  ],
  entryComponents: [
    ViewCarDealDialogComponent,
    ReservationDialogComponent,
    EditrentalcompanydialogComponent,
    RateVehicleDialogComponent,
    AddVehicleDialogComponent,
    CompanyIncomesDialogComponent,
    RemoveDestinationsDialogComponent,
    ChangeVehicleLocationDialogComponent,
    CompanyReservationsDialogComponent,
    EditAvioCompanyDialogComponent,
    EditCarDialogComponent,
    CompanyAddSuccessfullDialogComponent,
    EditFlightDialogComponent,
    EditrentalcompanydialogComponent,
    RateVehicleDialogComponent,
    ReservationDialogComponent,
    SelectDatesDialogComponent,
    ChangeHeadofficeDialogComponent,
    ThankYouDialogComponent,
    ThankYouForRateDialogComponent,
    VehiclesOnDiscountDialogComponent,
    DiscountedVehicleComponent,
    GraphicTicketDialogComponent,
    IncomesAviocompanyDialogComponent,
    DestinationsDialogComponent,
    AddNewBranchDialogComponent,
    EditHeadofficeDialogComponent,
    RateflightdialogComponent,
    RentacaroptiondialogComponent,
    DiscountedVehicleChooseDialogComponent,
    DiscountedVehicleDealsDialogComponent,
    BookDiscountedVehicleDialogComponent,
    HowManyDaysDialogComponent
  ]
})
export class DialogsModule { }
