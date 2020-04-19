import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Destination } from '../_models/destination';
import { Observable } from 'rxjs';
import { AvioCompany } from '../_models/aviocompany';
import { Flight } from '../_models/flight';
import { faUnderline } from '@fortawesome/free-solid-svg-icons';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/internal/operators/map';
import { CompanyToMake } from '../_models/companytomake';
import { FlightToMake } from '../_models/flightToMake';

@Injectable({
  providedIn: 'root'
})
export class AvioService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  makeNewFlight(companyId: number, newFlight: FlightToMake) {
    return this.http.post(this.baseUrl + 'avio/addFlight/' + companyId, newFlight);
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

  makeNewCompany(newCompany: CompanyToMake) {
    return this.http.post(this.baseUrl + 'avio/addCompany', newCompany);
  }

 
  // tslint:disable-next-line: max-line-length
  makeFlightReservation(email: string, username: string, departureDate: Date, arrivalDate: Date, departureDestination: string, arrivalDestination: string,
                        price: number, travelLength: number, seats: string)
                         {console.log(price, travelLength, seats);
                           return this.http.post(this.baseUrl + 'reservations/flightreservation',
                                                    {email});
}

}
