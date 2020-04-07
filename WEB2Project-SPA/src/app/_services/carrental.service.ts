import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarCompany } from '../_models/carcompany';
import { Vehicle } from '../_models/vehicle';
import { Reservation } from '../_models/carreservation';
import { CarCompanyReservationStats } from '../_models/carcompanyresstats';
import { CarCompanyIncomeStats } from '../_models/carcompanyincomestats';
import { DiscountedVehicle } from '../_models/discountedvehicle';

@Injectable({
  providedIn: 'root'
})
export class CarrentalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  
  getAllCarCompanies(): Observable<CarCompany[]> {
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

  getDiscountedVehiclesForCompany(companyId): Observable<DiscountedVehicle[]> {
    return this.http.get<DiscountedVehicle[]>(this.baseUrl + 'rentacar/getDiscountedVehicles/' + companyId);
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

  makeReservation(vehicleId: number, username: string, startdate: string,
                  enddate: string, totaldays: string, totalprice: string, companyname: string, 
                  companyid: string) {return this.http.post(this.baseUrl + 'reservations',
     {vehicleId, username, startdate, enddate, totaldays, totalprice, companyname, companyid});
  }

  rateVehicle(vehicleId: number, rating: string) {
    return this.http.post(this.baseUrl + 'rentacar/rateVehicle/' + vehicleId, {rating});
  }

  rateCompany(companyId: number, rating: string) {
    return this.http.post(this.baseUrl + 'rentacar/rateCompany/' + companyId, {rating});
  }

  addVehicle(vehicle: Vehicle, companyId: number) {
    return this.http.post(this.baseUrl + 'rentacar/newVehicle/' + companyId, vehicle);
  }

  editVehicle(vehicle: Vehicle) {
    return this.http.post(this.baseUrl + 'rentacar/editVehicle/' + vehicle.id, vehicle);
  }

  removeVehicle(id: number): Observable<Vehicle> {
    return this.http.get<Vehicle>(this.baseUrl + 'rentacar/deleteVehicle/' + id);
  }

  getReservationsStats(companyId: number): Observable<CarCompanyReservationStats> {
    return this.http.get<CarCompanyReservationStats>(this.baseUrl + 'rentacar/getReservations/' + companyId);
  }

  getIncomeStats(companyId: number, startingDate: string, finalDate: string): Observable<CarCompanyIncomeStats> { 
    return this.http.post<CarCompanyIncomeStats>(this.baseUrl + 'rentacar/getIncomes/' + companyId, {startingDate, finalDate});
  }

}
