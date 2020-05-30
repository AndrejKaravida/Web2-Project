import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Destination } from '../_models/_avioModels/destination';
import { AvioService } from '../_services/avio.service';
import { AlertifyService } from '../_services/alertify.service';
import { CarCompany } from '../_models/_carModels/carcompany';
import { AvioCompany } from '../_models/_avioModels/aviocompany';
import { CarrentalService } from '../_services/carrental.service';
import { Branch } from '../_models/_shared/branch';
import { SearchFlightDialogComponent } from '../_dialogs/_avio_company_dialogs/search-flight-dialog/search-flight-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Flight } from '../_models/_avioModels/flight';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  location = '';
  startingLocation = '';
  returningLocation = '';
  startingDate = new Date();
  returningDate = new Date();
  startingMinDate = new Date();
  returningMinDate = new Date();
  destinations: Destination[];
  branches: Branch[];
  avioCompanies: AvioCompany[];
  rentaCarCompanies: CarCompany[];
  company: AvioCompany;


  constructor(public authService: AuthService, private avioService: AvioService,
              private alertify: AlertifyService, private rentalService: CarrentalService, private dialog: MatDialog) { }

  ngOnInit() {
    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
    this.returningDate.setDate(this.returningDate.getDate() + 15);
    this.loadDestinations();
    this.loadAvioCompanies();
    this.loadCarCompanies();
    this.loadBranches();
  }
  SearchFlights() {
      this.avioService.searchFlights( this.startingLocation, this.returningLocation,this.startingDate.toLocaleDateString(),
         this.returningDate.toLocaleDateString()).subscribe(res => {
        if (res) {
          console.log(res);
          this.dialog.open(SearchFlightDialogComponent, {
            width: '1200px',
            height: '500px',
            data: {res}
          });
        } else {
          this.alertify.warning('There are no flighs with such criteria.')
        }
      });
  }

  loadBranches() {
    this.rentalService.getBranches().subscribe(res => {
      this.branches = res;
      this.location = res[0].city;
    });
  }

  searchCarCompanies() {
   this.rentalService.getCompaniesWithCriteria(this.location,
    this.startingDate.toLocaleDateString(), this.returningDate.toLocaleDateString()).subscribe(res => { 
      this.rentaCarCompanies = res;
    });
  }

  loadDestinations() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.startingLocation = res[0].city;
      this.returningLocation = res[1].city;
    }, err => {
      this.alertify.error('Error loading destinations!');
    });
  }

  loadAvioCompanies() { 
    this.avioService.getAllAvioCompanies().subscribe(res => { 
      this.avioCompanies = res;
    }, error => {
      let errorMessage = '';

      for (const err of error.error.errors) {
     errorMessage += err.message;
     errorMessage += '\n';
    }
      this.alertify.error(errorMessage);
    });
  }

  loadCarCompanies() { 
    this.rentalService.getAllCarCompanies().subscribe(res => { 
      this.rentaCarCompanies = res;
    }, error => {
      let errorMessage = '';

      for (const err of error.error.errors) {
     errorMessage += err.message;
     errorMessage += '\n';
    }
      this.alertify.error(errorMessage);
    });
  }

  onSearchByName(filterValue: string){
    const searchQuery = filterValue;

    let searchResults: CarCompany[] = [];
  

    for(let i = 0; i < this.rentaCarCompanies.length; i++) { 
      if(this.rentaCarCompanies[i].name.toLowerCase().includes(searchQuery.toLowerCase())){  
          searchResults.push(this.rentaCarCompanies[i]);
      }
    }

    this.rentaCarCompanies = searchResults;

    if(searchQuery === ''){
      this.loadCarCompanies();
    }
  }

  onSortCompanies() { 
      // tslint:disable-next-line: only-arrow-functions
      this.avioCompanies.sort((a, b) => a.name.localeCompare(b.name));
  }
  sortAvioAddress() {
    this.avioCompanies.sort((c, d) => c.headOffice.city.localeCompare(d.headOffice.city));
  }

  sortCarName() {
    this.rentaCarCompanies.sort((a, b) => a.name.localeCompare(b.name));
  }
  onSortCarCompanies() {
    this.rentaCarCompanies.sort((a, b) => a.address.localeCompare(b.address));
  }

}
