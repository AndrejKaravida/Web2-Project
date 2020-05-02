
import { Resolve, Router, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Vehicle } from '../_models/_carModels/vehicle';
import { CarrentalService } from '../_services/carrental.service';

@Injectable()
export class VehicleListResolver implements Resolve<Vehicle[]> {
    pageNumber = 1;
    pageSize = 5;

    constructor(private rentalService: CarrentalService,
                private router: Router, private alertify: AlertifyService) {}

     resolve(route: ActivatedRouteSnapshot): Observable<Vehicle[]> {
        return this.rentalService.getVehiclesForCompany(route.params.id, this.pageNumber, this.pageSize).pipe(
                        catchError(() => {
                            this.alertify.error('Problem retrieving data!');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
        }

}
