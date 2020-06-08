import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RentacarProfileComponent } from './rentacar-profile/rentacar-profile.component';
import { RentaCarProfileResolver } from './_resolvers/rentacar-profil-resolver';
import { VehicleListResolver } from './_resolvers/rentacar-vehicle-resolver';
import { AviocompanyProfileComponent } from './aviocompany-profile/aviocompany-profile.component';
import { AvioFlightsResolver } from './_resolvers/avio-flights-resolver';
import { AvioProfileResolver } from './_resolvers/avio-profile-resolver';
import { InterceptorService } from './_services/interceptor.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { PasswordGuard } from './_guards/password.guard';

const routes: Routes = [

  {
    path: 'home',
    component: HomeComponent,
    canActivate : [PasswordGuard]
  },
  {
    path: 'rentalprofile/:id',
    component: RentacarProfileComponent,
    resolve: {carcompany: RentaCarProfileResolver, vehicles: VehicleListResolver},
    canActivate : [PasswordGuard]
  },
  {
    path: 'avioprofile/:id',
    component: AviocompanyProfileComponent,
    resolve: {company: AvioProfileResolver, flights: AvioFlightsResolver},
    canActivate : [PasswordGuard]
  },
  {
    path: 'adminpanel',
    loadChildren: './admin-panel/admin.module#AdminModule'
  },
  {
    path: '',
    loadChildren: './user.module#UserModule'
  },

  {
    path: '**',
    redirectTo: 'home'
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {scrollPositionRestoration: 'top', useHash: false})],
  exports: [RouterModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: InterceptorService,
      multi: true
    }
  ]
})
export class AppRoutingModule {}