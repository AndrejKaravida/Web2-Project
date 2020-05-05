import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { AvioCompany } from '../_models/_avioModels/aviocompany';
import { AvioService } from '../_services/avio.service';

@Injectable()
export class AvioProfileResolver implements Resolve<AvioCompany> {

    constructor(private avioService: AvioService,
                private router: Router, private alertify: AlertifyService) {}

     resolve(route: ActivatedRouteSnapshot): Observable<AvioCompany> {
        const key = 'id';
        return this.avioService.getAvioCompany(route.params[key]).pipe(
                        catchError(() => {
                            this.alertify.error('Problem retrieving data!');
                            this.router.navigate(['/home']);
                            return of(null);
                        })
                    );
        }

}
