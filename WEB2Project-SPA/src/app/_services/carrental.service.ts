import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarCompany } from '../_models/carcompany';

@Injectable({
  providedIn: 'root'
})
export class CarrentalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCarRentalCompanies(): Observable<CarCompany[]> {
    return this.http.get<CarCompany[]>(this.baseUrl + 'rentacar');
  }

  getCarRentalCompany(id: number): Observable<CarCompany> {
    return this.http.get<CarCompany>(this.baseUrl + 'rentacar/' + id);
  }


}
