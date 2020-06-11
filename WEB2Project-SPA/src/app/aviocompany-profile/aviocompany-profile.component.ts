import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AvioCompany } from '../_models/_avioModels/aviocompany';
import { AvioService } from '../_services/avio.service';
import { Destination } from '../_models/_avioModels/destination';
import { AlertifyService } from '../_services/alertify.service';
// tslint:disable-next-line: max-line-length
import { EditAvioCompanyDialogComponent } from '../_dialogs/_avio_company_dialogs/edit-avio-company-dialog/edit-avio-company-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { EditFlightDialogComponent } from '../_dialogs/_avio_company_dialogs/edit-flight-dialog/edit-flight-dialog.component';
import { Pagination, PaginatedResult } from '../_models/_shared/pagination';
import { Flight } from '../_models/_avioModels/flight';
import { ShowMapDialogComponent } from '../_dialogs/_rent_a_car_dialogs/show-map-dialog/show-map-dialog.component';
import { GraphicTicketDialogComponent } from '../_dialogs/_avio_company_dialogs/graphic-ticket-dialog/graphic-ticket-dialog.component';
import { DestinationsDialogComponent } from '../_dialogs/_avio_company_dialogs/destinations-dialog/destinations-dialog.component';
import { EditHeadofficeDialogComponent } from '../_dialogs/_avio_company_dialogs/edit-headoffice-dialog/edit-headoffice-dialog.component';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';
import { SelectDatesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/select-dates-dialog/select-dates-dialog.component';
import { CompanyIncomesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/company-incomes-dialog/company-incomes-dialog.component';
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

  isAuth$: Observable<boolean>;
  role$: Observable<string>;
  isAdmin = false;

  constructor(private route: ActivatedRoute, private avioService: AvioService,
              private alertify: AlertifyService, private dialog: MatDialog,
              private router: Router, private store: Store<fromRoot.State>) { }


  ngOnInit() {


    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.role$ = this.store.select(fromRoot.getRole);

    this.route.data.subscribe(data => {
      this.company = data.company;
      this.flights = data.flights.result;
      this.pagination = data.flights.pagination;

      this.role$.subscribe(res => {
        if ((res === 'managerAvioNo' + data.company.id) || res === 'sysadmin') {
          this.isAdmin = true;
        }
      });
    });

    this.loadDestinations();

    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
    this.minPriceChosen = 0;
    this.maxPriceChosen = 1000;
  }

  loadDestinations() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.startingLocation = this.destinations[0].city;
      this.returningLocation = this.destinations[1].city;
    });
  }

  loadFlights() {
    if ((this.returningDate < this.startingDate) && this.twoWay === true) {
      alert('Returning date cannot be before the departure date!');
    } else {
      this.flightParams.minPrice = this.minPriceChosen;
      this.flightParams.maxPrice = this.maxPriceChosen;
      this.flightParams.departureDestination = this.startingLocation;
      this.flightParams.arrivalDestination = this.returningLocation;
      this.flightParams.departureDate = this.startingDate.toLocaleDateString();
      this.flightParams.returningDate = this.returningDate.toLocaleDateString();

      if (this.twoWay === false) {
        this.flightParams.returningDate = '';
      }
      this.route.params.subscribe(res => {
        this.avioService.getFlightsForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.flightParams)
        // tslint:disable-next-line: no-shadowed-variable
        .subscribe((res: PaginatedResult<Flight[]>) => {
          this.flights = res.result;
          this.pagination = res.pagination;
        }, error => {
          this.alertify.error('Failed to load flights!');
        });
      });
    }
  }

  nextPage() {
    this.route.params.subscribe(res => {
      this.avioService.getFlightsForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage)
      // tslint:disable-next-line: no-shadowed-variable
      .subscribe((res: PaginatedResult<Flight[]>) => {
        this.flights = res.result;
        this.pagination = res.pagination;
      }, error => {
        let errorMessage = '';

        for (const err of error.error.errors) {
       errorMessage += err.message;
       errorMessage += '\n';
      }
        this.alertify.error(errorMessage);
    });
  });
}

  resetFilters() {
    this.route.data.subscribe(data => {
      this.flights = data.flights.result;
      this.pagination = data.flights.pagination;
    });
  }

  onChangeHeadOffice() {
    const dialogRef = this.dialog.open(EditHeadofficeDialogComponent, {
      width: '450px',
      height: '350px',
      data: {id: this.company.id, destinations: this.destinations}
    });

    dialogRef.afterClosed().subscribe(_ => {
      this.loadCompany();
   });
  }
  OnFlightEdit() {
    const dialogRef = this.dialog.open(EditFlightDialogComponent, {
      width: '550px',
      height: '850px',
      data: {id: this.company.id, flightForSend: this.flight}
    });


    dialogRef.afterClosed().subscribe(result => {
   });
  }

  destinationsEdit() {
    const dialogRef = this.dialog.open(DestinationsDialogComponent, {
      width: '550px',
      height: '850px',
      data: {...this.company}
    });
  }

  ViewGraphic() {
    const dialogRef = this.dialog.open(GraphicTicketDialogComponent, {
      width: '550px',
      height: '400px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
   });
  }
  loadCompany() {
    this.avioService.getAvioCompany(this.company.id).subscribe(res => {
      this.company = res;
    }, err => {
      this.alertify.error('Error loading company data!');
    });
  }

  onCompanyEdit() {
    const dialogRef = this.dialog.open(EditAvioCompanyDialogComponent, {
      width: '550px',
      height: '600px',
      data: {...this.company}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.avioService.editAirComapny(result.id, result.name, result.promoDescription).subscribe(res => {
        this.company.name = result.name;
        this.company.promoDescription = result.promoDescription;
        this.alertify.success('You have successfully changed avio data.');
        dialogRef.close();
      });
    });
  }

  buyTicket() {
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

  onAvioCompanyIncomes() {
    const dialogRef = this.dialog.open(SelectDatesDialogComponent, {
      width: '450px',
      height: '350px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      // tslint:disable-next-line: max-line-length
      this.avioService.getAvioIncomes(this.company.id, result.startingDate.toLocaleDateString(), result.finalDate.toLocaleDateString()).subscribe(res => {
        this.dialog.open(CompanyIncomesDialogComponent, {
          width: '900px',
          height: '555px',
          data: {...res}
        });
      });
   });
  }

}
