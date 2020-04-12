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
