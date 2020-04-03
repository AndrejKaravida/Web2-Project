import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Destination } from '../_models/destination';
import { AvioService } from '../_services/avio.service';
import { AlertifyService } from '../_services/alertify.service';
import { CarCompany } from '../_models/carcompany';
import { AvioCompany } from '../_models/aviocompany';
import { CarrentalService } from '../_services/carrental.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  startingLocation = '';
  returningLocation = '';
  startingDate = new Date();
  returningDate = new Date();
  startingMinDate = new Date();
  returningMinDate = new Date();
  destinations: Destination[];
  avioCompanies: AvioCompany[];
  rentaCarCompanies: CarCompany[];


  constructor(public authService: AuthService, private avioService: AvioService,
              private alertify: AlertifyService, private rentalService: CarrentalService) { }

  ngOnInit() {
    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
    this.returningDate.setDate(this.returningDate.getDate() + 7);
    this.loadDestinations();
    this.loadAvioCompanies();
    this.loadCarCompanies();
  }

  loadDestinations() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.startingLocation = res[0].city + ' ' + res[0].country;
      this.returningLocation = res[1].city + ' ' + res[1].country;
    }, err => {
      this.alertify.error('Error loading destinations!');
    });
  }

  loadAvioCompanies() { 
    this.avioService.getAllAvioCompanies().subscribe(res => { 
      this.avioCompanies = res;
    }, error => {
      this.alertify.error('Error while loading avio companies!');
    });
  }

  loadCarCompanies() { 
    this.rentalService.getAllCarCompaniesNoPaging().subscribe(res => { 
      this.rentaCarCompanies = res;
    }, error => {
      this.alertify.error('Error while loading avio companies!');
    });
  }

  FilterFlights(){
    
  }

}
