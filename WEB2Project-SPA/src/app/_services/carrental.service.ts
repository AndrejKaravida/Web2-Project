import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarCompany } from '../_models/_carModels/carcompany';
import { Vehicle } from '../_models/_carModels/vehicle';
import { Reservation } from '../_models/_carModels/carreservation';
import { CarCompanyReservationStats } from '../_models/_carModels/carcompanyresstats';
import { PaginatedResult } from '../_models/_shared/pagination';
import { map } from 'rxjs/operators';
import { CompanyToMake } from '../_models/_carModels/companytomake';
import { Branch } from '../_models/_shared/branch';

@Injectable({
  providedIn: 'root'
})
export class CarrentalService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllCarCompanies(): Observable<CarCompany[]> {
    return this.http.get<CarCompany[]>(this.baseUrl + 'rentacar/carcompanies');
  }

  getVehiclesForCompany(companyId, page?, itemsPerPage?, vehicleParams?): Observable<PaginatedResult<Vehicle[]>> {
    let params = new HttpParams();
    const paginatedResult: PaginatedResult<Vehicle[]> = new PaginatedResult<Vehicle[]>();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (vehicleParams !== null && vehicleParams !== undefined) {
      params = params.append('minPrice', vehicleParams.minPrice);
      params = params.append('maxPrice', vehicleParams.maxPrice);

      params = params.append('twoseats', vehicleParams.seats.two);
      params = params.append('fiveseats', vehicleParams.seats.five);
      params = params.append('sixseats', vehicleParams.seats.six);

      params = params.append('twodoors', vehicleParams.doors.two);
      params = params.append('fourdoors', vehicleParams.doors.four);
      params = params.append('fivedoors', vehicleParams.doors.five);

      params = params.append('smalltype', vehicleParams.cartype.small);
      params = params.append('mediumtype', vehicleParams.cartype.medium);
      params = params.append('largetype', vehicleParams.cartype.large);
      params = params.append('luxurytype', vehicleParams.cartype.luxury);

      params = params.append('sevenrating', vehicleParams.averageRating.seven);
      params = params.append('eightrating', vehicleParams.averageRating.eight);
      params = params.append('ninerating', vehicleParams.averageRating.nine);
      params = params.append('tenrating', vehicleParams.averageRating.ten);

      params = params.append('pickupLocation', vehicleParams.pickupLocation);
      params = params.append('startingDate', vehicleParams.startingDate);
      params = params.append('returningDate', vehicleParams.returningDate);
    }

    return this.http.get<Vehicle[]>(this.baseUrl + 'rentacar/getVehicles/' + companyId, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  getDiscountedVehiclesForCompany(companyId): Observable<Vehicle[]> {
    return this.http.get<Vehicle[]>(this.baseUrl + 'rentacar/getDiscountedVehicles/' + companyId);
  }

  getCarReservationsForUser(username: string): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(this.baseUrl + 'reservations/' + username);
  }

  getCarRentalCompany(id: number): Observable<CarCompany> {
    return this.http.get<CarCompany>(this.baseUrl + 'rentacar/' + id);
  }

  updateComapny(company: CarCompany) {
    return this.http.post(this.baseUrl + 'rentacar/editCompany', company);
  }

  makeReservation(vehicleId: number, username: string, startdate: string,
                  enddate: string, totaldays: string, totalprice: string, companyname: string,
                  companyid: string, startingLocation: string, returningLocation: string) {return this.http.post(this.baseUrl + 'reservations',
     {returningLocation, vehicleId, username, startdate, enddate, totaldays, totalprice, companyname, companyid, startingLocation});
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

  makeNewCompany(newCompany: CompanyToMake) {
    return this.http.post(this.baseUrl + 'rentacar/addCompany', newCompany);
  }

  addNewBranch(companyId: number, branch: Branch) {
    return this.http.post(this.baseUrl + 'rentacar/addNewBranch/' + companyId, branch);
  }

  editVehicle(vehicle: Vehicle, companyId: number) {
    return this.http.post(this.baseUrl + 'rentacar/editVehicle/' + vehicle.id + '/' + companyId, vehicle);
  }

  changeVehicleLocation(vehicleId: number, newCity: string, companyId: string) {
    return this.http.post(this.baseUrl + 'rentacar/changeVehicleLocation/' + vehicleId, {newCity, companyId});
  }

  removeVehicle(id: number, companyId: number): Observable<Vehicle> {
    return this.http.post<Vehicle>(this.baseUrl + 'rentacar/deleteVehicle/' + id, {companyId});
  }

  getReservationsStats(companyId: number): Observable<CarCompanyReservationStats> {
    return this.http.get<CarCompanyReservationStats>(this.baseUrl + 'rentacar/getReservations/' + companyId);
  }

  getIncomeStats(companyId: number, startingDate: string, finalDate: string) {
    return this.http.post(this.baseUrl + 'rentacar/getIncomes/' + companyId, {startingDate, finalDate});
  }

  changeHeadOffice(companyId, headOffice: string) {
    return this.http.post(this.baseUrl + 'rentacar/changeHeadOffice/' + companyId, {headOffice});
  }

  removeCompanyLocation(companyId, location: string) {
    return this.http.post(this.baseUrl + 'rentacar/removeDestination/' + companyId, {location});
  }

  canEditVehicle(vehicleId: number): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'rentacar/canEdit/' + vehicleId);
  }

  canRemoveLocation(companyId: number, location: string): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'rentacar/canRemoveLocation/' + companyId, {location});
  }

}
