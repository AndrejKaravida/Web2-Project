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
import { FlightReservationComponent } from './flight-reservation/flight-reservation.component';

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
    canActivate: [AuthGuard]
  },
  {
    path: 'myreservations',
    component: ReservationsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'flightreservations',
    component: FlightReservationComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
