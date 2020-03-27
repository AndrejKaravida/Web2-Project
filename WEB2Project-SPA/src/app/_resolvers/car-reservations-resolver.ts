import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { CarCompany } from '../_models/carcompany';
import { CarrentalService } from '../_services/carrental.service';
import { Reservation } from '../_models/carreservation';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class CarReservationsResolver implements Resolve<Reservation[]> {
    name: string;

    constructor(private rentalService: CarrentalService, private authService: AuthService,
                private router: Router, private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Reservation[]> {

    this.authService.userProfile$.subscribe(res => {
        if(res){
            this.name = res.name;
        }
    });

    return this.rentalService.getCarReservationsForUser(this.name).pipe(
                catchError(() => {
                    this.alertify.error('Problem retrieving data!');
                    this.router.navigate(['/home']);
                    return of(null);
                })
            );
 }

}

