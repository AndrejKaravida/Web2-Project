import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarCompany } from '../_models/carcompany';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Vehicle } from '../_models/vehicle';
import { Reservation } from '../_models/carreservation';

@Injectable({
  providedIn: 'root'
})
export class CarrentalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCarRentalCompanies(page?, itemsPerPage?, companyParams?): Observable<PaginatedResult<CarCompany[]>> {

    const paginatedResult: PaginatedResult<CarCompany[]> = new PaginatedResult<CarCompany[]>();

    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (companyParams !== null && companyParams !== undefined) {
      params = params.append('minPrice', companyParams.minPrice);
      params = params.append('maxPrice', companyParams.maxPrice);
      params = params.append('minSeats', companyParams.minPrice);
      params = params.append('maxSeats', companyParams.maxPrice);
      params = params.append('minDoors', companyParams.minPrice);
      params = params.append('maxDoors', companyParams.maxPrice);
    }

    return this.http.get<CarCompany[]>(this.baseUrl + 'rentacar', {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getAllCarCompaniesNoPaging(): Observable<CarCompany[]> {
    return this.http.get<CarCompany[]>(this.baseUrl + 'rentacar/carcompanies');
  }
  
  getVehiclesForCompany(companyId, companyParams?): Observable<Vehicle[]> {

    let params = new HttpParams();

    if (companyParams !== null && companyParams !== undefined) {
      params = params.append('minPrice', companyParams.minPrice);
      params = params.append('maxPrice', companyParams.maxPrice);
      params = params.append('minSeats', companyParams.minSeats);
      params = params.append('maxSeats', companyParams.maxSeats);
      params = params.append('minDoors', companyParams.minDoors);
      params = params.append('maxDoors', companyParams.maxDoors);
      params = params.append('averageRating', companyParams.averageRating);
      params = params.append('types', companyParams.type);
    }

    return this.http.get<Vehicle[]>(this.baseUrl + 'rentacar/getVehicles/' + companyId, {params});
  }

  getVehiclesForCompanyNoParams(companyId): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(this.baseUrl + 'rentacar/getVehiclesNoParams/' + companyId);
  }

  getCarReservationsForUser(username: string): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(this.baseUrl + 'reservations/' + username);
  }

  getCarRentalCompany(id: number): Observable<CarCompany> {
    return this.http.get<CarCompany>(this.baseUrl + 'rentacar/' + id);
  }

  updateComapny(company: CarCompany) {
    return this.http.post(this.baseUrl + 'rentacar', company);
  }

  makeReservation(vehicleId: number, username: string, startdate: string, enddate: string, totaldays:string, totalprice: string, companyname: string, companyid: string) {
    return this.http.post(this.baseUrl + 'reservations', {vehicleId, username, startdate, enddate, totaldays, totalprice, companyname, companyid});
  }

  rateVehicle(vehicleId: number, rating: string) {
    return this.http.post(this.baseUrl + 'rentacar/rateVehicle/' + vehicleId, {rating});
  }

  rateCompany(companyId: number, rating: string) {
    return this.http.post(this.baseUrl + 'rentacar/rateCompany/' + companyId, {rating});
  }

  addVehicle(vehicle: Vehicle) {
    return this.http.post(this.baseUrl + 'rentacar/newVehicle', vehicle);
  }


}
