
import { Resolve, Router, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Vehicle } from '../_models/vehicle';
import { CarrentalService } from '../_services/carrental.service';

@Injectable()
export class VehicleListResolver implements Resolve<Vehicle[]> {

    constructor(private rentalService: CarrentalService,
                private router: Router, private alertify: AlertifyService) {}

     resolve(route: ActivatedRouteSnapshot): Observable<Vehicle[]> {
        return this.rentalService.getVehiclesForCompanyNoParams(route.params.id).pipe(
                        catchError(() => {
                            this.alertify.error('Problem retrieving data!');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
        }

}
