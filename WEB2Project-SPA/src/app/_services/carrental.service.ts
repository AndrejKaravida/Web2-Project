import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarCompany } from '../_models/carcompany';

@Injectable({
  providedIn: 'root'
})
export class CarrentalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCarRentalCompanies(companyParams?): Observable<CarCompany[]> {


    let params = new HttpParams();

    if (companyParams !== null && companyParams !== undefined) {
      params = params.append('minPrice', companyParams.minPrice);
      params = params.append('maxPrice', companyParams.maxPrice);
      params = params.append('minSeats', companyParams.minPrice);
      params = params.append('maxSeats', companyParams.maxPrice);
      params = params.append('minDoors', companyParams.minPrice);
      params = params.append('maxDoors', companyParams.maxPrice);
    }
    
    return this.http.get<CarCompany[]>(this.baseUrl + 'rentacar');
  }

  getCarRentalCompany(id: number): Observable<CarCompany> {
    return this.http.get<CarCompany>(this.baseUrl + 'rentacar/' + id);
  }

  updateComapny(company: CarCompany) {
    return this.http.post(this.baseUrl + 'rentacar', company);
  }


}
