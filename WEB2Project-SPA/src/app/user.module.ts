import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { ProfileComponent } from './profile/profile.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from './material.module';
import { DiscountFlightCardComponent } from './aviocompany-profile/discount-flight-card/discount-flight-card.component';
import { DiscountTicketListsComponent } from './aviocompany-profile/discount-ticket-lists/discount-ticket-lists.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ChartsModule } from 'ng2-charts';
import { DialogsModule } from './dialogs.module';
import { ReservationsComponent } from './reservations/reservations.component';


@NgModule({
  declarations: [
    ProfileComponent,
    DiscountFlightCardComponent,
    ReservationsComponent,
    DiscountTicketListsComponent,
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    FontAwesomeModule,
    DialogsModule,
    ChartsModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    UserRoutingModule
  ]
})
export class UserModule { }
