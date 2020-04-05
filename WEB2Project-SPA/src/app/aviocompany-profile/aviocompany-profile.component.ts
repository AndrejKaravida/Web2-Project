import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AvioCompany } from '../_models/aviocompany';
import { AvioService } from '../_services/avio.service';
import { Destination } from '../_models/destination';
import { AlertifyService } from '../_services/alertify.service';
import { Flight } from '../_models/flight';

@Component({
  selector: 'app-aviocompany-profile',
  templateUrl: './aviocompany-profile.component.html',
  styleUrls: ['./aviocompany-profile.component.css']
})


export class AviocompanyProfileComponent implements OnInit {

  company: AvioCompany;
  destinations: Destination[];

  startingDate = new Date();
  returningDate = new Date();

  startingMinDate = new Date();
  returningMinDate = new Date();

  minPriceChosen: number;
  maxPriceChosen: number;

  flightParams: any = {};

  startingLocation = '';
  returningLocation = '';

  twoWay = false;

  constructor(private route: ActivatedRoute, private avioService: AvioService,
              private alertify: AlertifyService) { }


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.avioService.getAvioCompany(params.id).subscribe(res => {
        this.company = res;
        this.minPriceChosen = 0;
        this.maxPriceChosen = 1000;
        this.loadDestinations();
      });
    });

    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
  }

  loadFlights(){
    this.flightParams.minPrice = this.minPriceChosen;
    this.flightParams.maxPrice = this.maxPriceChosen;
    this.flightParams.departureDestination = this.startingLocation;
    this.flightParams.arrivalDestination = this.returningLocation;
    this.flightParams.departureDate = this.startingDate.toLocaleDateString();
    this.flightParams.returningDate = this.returningDate.toLocaleDateString();

    if(this.twoWay === false) {
      this.flightParams.returningDate = '';
    }

    this.route.params.subscribe(res => {
      this.avioService.getFlightsForCompany(res.id, this.flightParams).subscribe(res => {
        this.company.flights = res;
      }, error => {
        this.alertify.error('Failed to load flights!');
      });
    });
  }

  resetFilters(){
    this.route.params.subscribe(res => {
      this.avioService.getAvioCompany(res.id).subscribe(res => {
        this.company.flights = res.flights;
      }, error => {
        this.alertify.error('Failed to load flights!');
      });
    });
    
  }

  loadDestinations() { 
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.startingLocation = this.destinations[0].city;
      this.returningLocation = this.destinations[1].city;
    });
  }

}
