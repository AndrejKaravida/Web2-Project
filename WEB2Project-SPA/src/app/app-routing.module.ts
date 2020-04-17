import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthGuard } from './_guards/auth.guard';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import { AviocompanyProfileComponent } from './aviocompany-profile/aviocompany-profile.component';

import { AvioFlightsResolver } from './_resolvers/avio-flights-resolver';
import { AvioProfileResolver } from './_resolvers/avio-profile-resolver';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { UpdateUserprofileDialogComponent } from './_dialogs/update-userprofile-dialog/update-userprofile-dialog.component';
import { AuthConfig } from 'angular-oauth2-oidc';
import { DiscountTicketListsComponent } from './aviocompany-profile/discount-ticket-lists/discount-ticket-lists.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'discounttickets/:id',
    component: DiscountTicketListsComponent,
    canActivate : [AuthGuard],
    resolve: {company: AvioProfileResolver}
  },
  {
    path: 'profile/profileupdate',
    component: UpdateUserprofileDialogComponent,
    canActivate : [AuthGuard]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'rentalprofile/:id',
    component: RentacarProfileComponent,
    resolve: {carcompany: RentaCarProfileResolver, vehicles: VehicleListResolver},
    canActivate: [AuthGuard]
  },
    {
    path: 'avioprofile/:id',
    component: AviocompanyProfileComponent,
    resolve: {company: AvioProfileResolver, flights: AvioFlightsResolver},
    canActivate: [AuthGuard]
  },
  {
    path: 'myreservations',
    component: ReservationsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'adminpanel',
    component: AdminPanelComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled', useHash: false})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
