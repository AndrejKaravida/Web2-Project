import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Destination } from '../_models/destination';
import { Observable } from 'rxjs';
import { AvioCompany } from '../_models/aviocompany';

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

  getAllDestinationsForCompany(id: number): Observable<Destination[]> {
    return this.http.get<Destination[]>(this.baseUrl + 'avio/destinations/' + id);
  }

  getAllAvioCompanies(): Observable<AvioCompany[]> {
    return this.http.get<AvioCompany[]>(this.baseUrl + 'avio/aircompanies/');
  }

 



}
