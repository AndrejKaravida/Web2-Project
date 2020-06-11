import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Destination } from '../_models/_avioModels/destination';
import { Observable } from 'rxjs';
import { AvioCompany } from '../_models/_avioModels/aviocompany';
import { Flight } from '../_models/_avioModels/flight';
import { faUnderline } from '@fortawesome/free-solid-svg-icons';
import { PaginatedResult } from '../_models/_shared/pagination';
import { map } from 'rxjs/internal/operators/map';
import { FlightToMake } from '../_models/_avioModels/flightToMake';
import { CompanyToMake } from '../_models/_carModels/companytomake';
import { FlightReservation } from '../_models/_avioModels/flightReservation';

@Injectable({
  providedIn: 'root'
})
export class AvioService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  editHeadOffice(companyId, headOffice: string) {
    return this.http.post(this.baseUrl + 'avio/editHeadOffice/' + companyId, {headOffice});
  }

  getAvioIncomes(companyId: number, startingDate: string, finalDate: string) {
    return this.http.post(this.baseUrl + 'avio/avioIncomes/' + companyId, {startingDate, finalDate});
  }
  getFlightReservationsForUser(authid: string): Observable<FlightReservation[]> {
    return this.http.get<FlightReservation[]>(this.baseUrl + 'reservations/flightReservations/' + authid);
  }

  makeNewFlight(companyId: number, newFlight: FlightToMake) {
    return this.http.post(this.baseUrl + 'avio/addFlight/' + companyId, newFlight);
  }

  editFlight(flightId: number, flightToEdit: Flight) {
    return this.http.post(this.baseUrl + 'avio/editFlight/' + flightId, flightToEdit);
  }

  getAllDestinations(): Observable<Destination[]> {
    return this.http.get<Destination[]>(this.baseUrl + 'avio/destinations/');
  }

  getAvioCompany(id: number): Observable<AvioCompany> {
    return this.http.get<AvioCompany>(this.baseUrl + 'avio/getCompany/' + id);
  }

  getAllAvioCompanies(): Observable<AvioCompany[]> {
    return this.http.get<AvioCompany[]>(this.baseUrl + 'avio/aircompanies/');
  }

  getDiscountedTickets(companyId: number): Observable<Flight[]> {
    return this.http.get<Flight[]>(this.baseUrl + 'avio/getDiscountedFlights/' + companyId);
  }

  getFlightsForCompany(companyId, page?, itemsPerPage?, flightParams?): Observable<PaginatedResult<Flight[]>>{

    const paginatedResult: PaginatedResult<Flight[]> = new PaginatedResult<Flight[]>();

    let params = new HttpParams();

    if (page !== null && itemsPerPage !== null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (flightParams != null && flightParams != faUnderline) { 
      params = params.append('minPrice', flightParams.minPrice);
      params = params.append('maxPrice', flightParams.maxPrice);
      params = params.append('departureDestination', flightParams.departureDestination);
      params = params.append('arrivalDestination', flightParams.arrivalDestination);
      params = params.append('departureDate', flightParams.departureDate);
      params = params.append('returningDate', flightParams.returningDate);
    }

    return this.http.get<Flight[]>(this.baseUrl + 'avio/getFlights/' + companyId, {observe: 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }
  editAirComapny(companyId: number, name: string, promodesc: string) {
    return this.http.post(this.baseUrl + 'avio/editcompany/' + companyId, {Name: name, PromoDescription: promodesc});
  }


  makeNewCompany(newCompany: CompanyToMake) {
    return this.http.post(this.baseUrl + 'avio/addCompany', newCompany);
  }

  getCompanyForFlight(id: number): Observable<AvioCompany> {
    return this.http.get<AvioCompany>(this.baseUrl + 'avio/checkCompany/' + id);
  }


  rate(flightId: number, companyRating: string, userId: string, reservationId: number, flightRating: string, companyId: number) {
    return this.http.post(this.baseUrl + 'avio/rate', {flightId, companyRating, userId, reservationId, flightRating, companyId});
  }

  searchFlights(departureDestination: string, arrivalDestination: string, departureTime: string, arrivalTime: string) {
    return this.http.post(this.baseUrl + 'avio/searchFlights',
   {
    startingDestination: departureDestination,  arrivalDestination,  departureDate: departureTime, arrivalDate: arrivalTime
   });
  }

  makeFlightReservation(authId: string, departureTime: Date, arrivalTime: Date, departureDestination: string, 
                        arrivalDestination: string, price: number, travelLength: number,
                        companyId: number, companyName: string, companyPhoto: string, flightId: string) {
                          return this.http.post(this.baseUrl + 'reservations/flightreservation', {authId,
                          departureDestination, departureTime, arrivalTime, arrivalDestination, price, travelLength,
                          companyId, companyName, companyPhoto, flightId});
                        }
}
