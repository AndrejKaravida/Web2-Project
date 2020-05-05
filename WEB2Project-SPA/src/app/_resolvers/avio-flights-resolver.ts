import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Flight } from '../_models/_avioModels/flight';
import { AvioService } from '../_services/avio.service';

@Injectable()
export class AvioFlightsResolver implements Resolve<Flight[]> {
    pageNumber = 1;
    pageSize = 6;

    constructor(private avioService: AvioService,
                private router: Router, private alertify: AlertifyService) {}

     resolve(route: ActivatedRouteSnapshot): Observable<Flight[]> {
        return this.avioService.getFlightsForCompany(route.params.id, this.pageNumber, this.pageSize).pipe(
                        catchError(() => {
                            this.alertify.error('Problem retrieving data!');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
        }

}
