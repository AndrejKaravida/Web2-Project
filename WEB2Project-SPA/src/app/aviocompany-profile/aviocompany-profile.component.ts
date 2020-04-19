import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AvioCompany } from '../_models/aviocompany';
import { AvioService } from '../_services/avio.service';
import { Destination } from '../_models/destination';
import { AlertifyService } from '../_services/alertify.service';
// tslint:disable-next-line: max-line-length
import { EditAvioCompanyDialogComponent } from '../_dialogs/edit-avio-company-dialog/edit-avio-company-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EditFlightDialogComponent } from '../_dialogs/edit-flight-dialog/edit-flight-dialog.component';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { Flight } from '../_models/flight';
import { ShowMapDialogComponent } from '../_dialogs/show-map-dialog/show-map-dialog.component';
import { GraphicTicketDialogComponent } from '../_dialogs/graphic-ticket-dialog/graphic-ticket-dialog.component';

@Component({
  selector: 'app-aviocompany-profile',
  templateUrl: './aviocompany-profile.component.html',
  styleUrls: ['./aviocompany-profile.component.css']
})

export class AviocompanyProfileComponent implements OnInit {



  company: AvioCompany;
  destinations: Destination[];

  pagination: Pagination;
  flight: Flight;
  flights: Flight[];

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
              private alertify: AlertifyService, private dialog: MatDialog,
              private router: Router) { }


  ngOnInit() {
    this.loadDestinations();
    this.route.data.subscribe(data => { 
      this.company = data.company;
      this.flights = data.flights.result;
      this.pagination = data.flights.pagination;
    });

    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
    this.minPriceChosen = 0;
    this.maxPriceChosen = 1000;
  }

  loadFlights() {
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
      this.avioService.getFlightsForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.flightParams)
      .subscribe((res: PaginatedResult<Flight[]>) => {
        this.flights = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error('Failed to load flights!');
      });
    });
  }

  nextPage() { 
    this.route.params.subscribe(res => {
      this.avioService.getFlightsForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe((res: PaginatedResult<Flight[]>) => {
        this.flights = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error('Failed to load flights!');
      });
    });
  }

  resetFilters(){
    this.route.data.subscribe(data => { 
      this.flights = data.flights.result;
      this.pagination = data.flights.pagination;
    });
  }

  loadDestinations() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.startingLocation = this.destinations[0].city;
      this.returningLocation = this.destinations[1].city;
    });
  }

  OnFlightEdit(){
    const dialogRef = this.dialog.open(EditFlightDialogComponent, {
      width: '550px',
      height: '850px',
      data: {id: this.company.id, flightForSend: this.flight}
    });

    dialogRef.afterClosed().subscribe(result => {
   });
  }
  ViewGraphic(){
    const dialogRef = this.dialog.open(GraphicTicketDialogComponent, {
      width: '550px',
      height: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
   });
  }
  onCompanyEdit() {
    const dialogRef = this.dialog.open(EditAvioCompanyDialogComponent, {
      width: '550px',
      height: '600px',
      data: {...this.company}
    });

    dialogRef.afterClosed().subscribe(result => {
   });
  }

  buyTicket()
  {
    this.alertify.success('You have booked travel.');
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.nextPage();
  }

  onShowMap() {
    this.dialog.open(ShowMapDialogComponent, {
      width: '1200px',
      height: '800px',
      data: {mapString: this.company.headOffice.mapString}
    });
  }

  onDiscountedFlights() { 
    this.router.navigate(['discounttickets/' + this.company.id]);
  }

}
