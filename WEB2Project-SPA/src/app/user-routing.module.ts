import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from './_guards/auth.guard';
import { PasswordGuard } from './_guards/password.guard';
import { DiscountTicketListsComponent } from './aviocompany-profile/discount-ticket-lists/discount-ticket-lists.component';
import { AvioProfileResolver } from './_resolvers/avio-profile-resolver';
import { ReservationsComponent } from './reservations/reservations.component';
import { ChangePasswordComponent } from './change-password/change-password.component';


const routes: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard, PasswordGuard],
  },
  {
    path: 'discounttickets/:id',
    component: DiscountTicketListsComponent,
    canActivate : [AuthGuard, PasswordGuard],
    resolve: {company: AvioProfileResolver}
  },
  {
    path: 'myreservations',
    component: ReservationsComponent,
    canActivate : [AuthGuard, PasswordGuard],
  },
  {
    path: 'changepassword',
    component: ChangePasswordComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
