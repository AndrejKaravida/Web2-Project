import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { CarCompany } from '../_models/_carModels/carcompany';
import { CarrentalService } from '../_services/carrental.service';

@Injectable()
export class RentaCarProfileResolver implements Resolve<CarCompany> {

    constructor(private rentalService: CarrentalService,
                private router: Router, private alertify: AlertifyService) {}

     resolve(route: ActivatedRouteSnapshot): Observable<CarCompany> {
        const key = 'id';
        return this.rentalService.getCarRentalCompany(route.params[key]).pipe(
                        catchError(() => {
                            this.alertify.error('Problem retrieving data!');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
        }

}

