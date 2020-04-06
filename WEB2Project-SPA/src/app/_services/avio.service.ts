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

@Injectable({
  providedIn: 'root'
})
export class AvioService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getAllDestinations(): Observable<Destination[]> {
    return this.http.get<Destination[]>(this.baseUrl + 'avio/destinations/');
  }

  getAvioCompany(id: number): Observable<AvioCompany> {
    return this.http.get<AvioCompany>(this.baseUrl + 'avio/getCompany/' + id);
  }

  getAllAvioCompanies(): Observable<AvioCompany[]> {
    return this.http.get<AvioCompany[]>(this.baseUrl + 'avio/aircompanies/');
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

}
