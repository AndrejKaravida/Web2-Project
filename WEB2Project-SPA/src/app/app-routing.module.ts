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
import { InterceptorService } from './_services/interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DiscountTicketListsComponent } from './aviocompany-profile/discount-ticket-lists/discount-ticket-lists.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

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
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'rentalprofile/:id',
    component: RentacarProfileComponent,
    resolve: {carcompany: RentaCarProfileResolver, vehicles: VehicleListResolver},
  },
    {
    path: 'avioprofile/:id',
    component: AviocompanyProfileComponent,
    resolve: {company: AvioProfileResolver, flights: AvioFlightsResolver},
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
  },
  {
    path: 'changepassword',
    component: ChangePasswordComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled', useHash: false})],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true
    }
  ]
})
export class AppRoutingModule { }