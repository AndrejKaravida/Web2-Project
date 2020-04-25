import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute } from '@angular/router';
import { EditrentalcompanydialogComponent } from '../_dialogs/_rent_a_car_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/carcompany';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';
import { ViewCarDealDialogComponent } from '../_dialogs/_rent_a_car_dialogs/viewCarDealDialog/viewCarDealDialog.component';
import { AddVehicleDialogComponent } from '../_dialogs/_rent_a_car_dialogs/add-vehicle-dialog/add-vehicle-dialog.component';
import { EditCarDialogComponent } from '../_dialogs/_rent_a_car_dialogs/edit-car-dialog/edit-car-dialog.component';
import { CompanyIncomesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/company-incomes-dialog/company-incomes-dialog.component';
import { CompanyReservationsDialogComponent } from '../_dialogs/_rent_a_car_dialogs/company-reservations-dialog/company-reservations-dialog.component';
import { CarCompanyReservationStats } from '../_models/carcompanyresstats';
import { SelectDatesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/select-dates-dialog/select-dates-dialog.component';
import { VehiclesOnDiscountDialogComponent } from '../_dialogs/_rent_a_car_dialogs/vehicles-on-discount-dialog/vehicles-on-discount-dialog.component';
import { ShowMapDialogComponent } from '../_dialogs/_rent_a_car_dialogs/show-map-dialog/show-map-dialog.component';
import { AddNewDestinationDialogComponent } from '../_dialogs/_rent_a_car_dialogs/add-new-destination-dialog/add-new-destination-dialog.component';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { ChangeHeadofficeDialogComponent } from '../_dialogs/_rent_a_car_dialogs/change-headoffice-dialog/change-headoffice-dialog.component';
import { RemoveDestinationsDialogComponent } from '../_dialogs/_rent_a_car_dialogs/remove-destinations-dialog/remove-destinations-dialog.component';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog, private alertify: AlertifyService,
              private store: Store<fromRoot.State>) { }
  rentalCompany: CarCompany;
  vehicles: Vehicle[];
  companyResStats: CarCompanyReservationStats;
  vehicleParams: any = {};
  averageRating: any = {};
  cartype: any = {};
  doors: any = {};
  seats: any = {};
  startingLocation = '';
  returningLocation = '';
  startingDate: Date;
  returningDate: Date;
  startingMinDate = new Date();
  returningMinDate = new Date();
  pagination: Pagination;
  disabled = true;
  isAuth$: Observable<boolean>;

  ngOnInit() {
    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.route.data.subscribe(data => {
      this.vehicles = data.vehicles.result;
      this.rentalCompany = data.carcompany;
      this.pagination = data.vehicles.pagination;
    });
    this.loadParametres();
    this.startingLocation = this.rentalCompany.destinations[0].city;
    this.returningLocation = this.rentalCompany.destinations[0].city;
  }

  onShowMap() {
    this.dialog.open(ShowMapDialogComponent, {
      width: '1200px',
      height: '800px',
      data: {mapString: this.rentalCompany.headOffice.mapString}
    });
  }

  loadVehicles() {

    if (this.startingDate == null || this.returningDate == null) {
      this.disabled = true;
      return;
    }
    this.disabled = false;

    if (this.averageRating.seven) {
    this.vehicleParams.averageRating = 7;
    } else if (this.averageRating.eight) {
    this.vehicleParams.averageRating = 8;
    } else if (this.averageRating.nine) {
    this.vehicleParams.averageRating = 9;
    } else if (this.averageRating.ten) {
    this.vehicleParams.averageRating = 9.5;
    } else {
      this.vehicleParams.averageRating = 6;
    }

    if (this.cartype.small) {
      this.vehicleParams.type = this.vehicleParams.type.concat('small,');
    }
    if (this.cartype.medium) {
      this.vehicleParams.type = this.vehicleParams.type.concat('medium,');
    }
    if (this.cartype.large) {
      this.vehicleParams.type = this.vehicleParams.type.concat('large,');
    }
    if (this.cartype.luxury) {
      this.vehicleParams.type = this.vehicleParams.type.concat('luxury');
    }


    if (this.doors.five) {
      this.vehicleParams.maxDoors = 7;
      this.vehicleParams.minDoors = 5;
     }
    if (this.doors.four) {
       this.vehicleParams.minDoors = 4;
       if (!this.doors.five) {
         this.vehicleParams.maxDoors = 4;
       }
   }
    if (this.doors.two) {
     this.vehicleParams.minDoors = 2;
     this.vehicleParams.maxDoors = 2;
     if (this.doors.four) {
       this.vehicleParams.maxDoors = 4;
     }
     if (this.doors.five) {
       this.vehicleParams.maxDoors = 7;
     }
   }

    if (!this.doors.two && !this.doors.four && !this.doors.five) {
     this.vehicleParams.minDoors = 0;
     this.vehicleParams.maxDoors = 0;
   }

    if (this.seats.six) {
       this.vehicleParams.maxSeats = 8;
       this.vehicleParams.minSeats = 6;
      }
    if (this.seats.five) {
        this.vehicleParams.minSeats = 3;
        if (!this.seats.six) {
          this.vehicleParams.maxSeats = 5;
        }
    }
    if (this.seats.two) {
      this.vehicleParams.minSeats = 1;
      this.vehicleParams.maxSeats = 2;
      if (this.seats.five) {
        this.vehicleParams.maxSeats = 5;
      }
      if (this.seats.six) {
        this.vehicleParams.maxSeats = 8;
      }
    }

    if (!this.seats.two && !this.seats.five && !this.seats.six) {
      this.vehicleParams.minSeats = 0;
      this.vehicleParams.maxSeats = 0;
    }

    this.vehicleParams.pickupLocation = this.startingLocation;
    this.vehicleParams.startingDate = this.startingDate.toLocaleDateString();
    this.vehicleParams.returningDate = this.returningDate.toLocaleDateString();

    this.route.params.subscribe(res => {
      // tslint:disable-next-line: no-shadowed-variable
      this.rentalService.getVehiclesForCompany(res.id,this.pagination.currentPage, 
        this.pagination.itemsPerPage, this.vehicleParams).subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error('Failed to load vehicles!');
      });
    });
  }

  resetFilters() {
    this.route.data.subscribe(data => {
      this.vehicles = data.vehicles.result;
      this.pagination = data.vehicles.pagination;
    });
    this.loadParametres();
  }

  loadCompany() {
    this.rentalService.getCarRentalCompany(this.rentalCompany.id).subscribe(res => {
      this.rentalCompany = res;
    }, err => {
      this.alertify.error('Error loading company data!');
    });
  }

  onEditCompany() {
    const dialogRef = this.dialog.open(EditrentalcompanydialogComponent, {
      width: '400px',
      height: '600px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        result.weekRentalDiscount = +result.weekRentalDiscount;
        result.monthRentalDiscount = +result.monthRentalDiscount;
        this.rentalService.updateComapny(result).subscribe(res => {
          this.alertify.success('Successfully changed company data!');
          this.loadCompany();
        });
      }
      }, err => {
        this.alertify.error('Problem editing company data!');
      });
  }

  onAddNewDestination() {
    const dialogRef = this.dialog.open(AddNewDestinationDialogComponent, {
      width: '500px',
      height: '550px',
      data: {id: this.rentalCompany.id}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.loadCompany();
    });
  }

  onViewDeal(vehicle: Vehicle) {

    if (this.startingDate == null || this.returningDate == null) {
      this.disabled = true;
      this.alertify.warning('Starting and returning dates cannot be blank!');
      return;
    }

    let diffc = this.returningDate.getTime() - this.startingDate.getTime();

    let days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));

    let discount = 0;
    let different = false;

    if (days > 29) {
      discount = this.rentalCompany.monthRentalDiscount;
    } else if (days > 6) {
      discount = this.rentalCompany.weekRentalDiscount;
    }

    let totalPrice = days * vehicle.price;

    if (discount > 0) {
      totalPrice = totalPrice - (totalPrice * (discount / 100));
    }

    if (this.startingLocation !== this.returningLocation) {
      totalPrice += 200;
      different = true;
    }


    this.dialog.open(ViewCarDealDialogComponent, {
      width: '400px',
      height: '680px',
      data: {companyName: this.rentalCompany.name,
             companyId: this.rentalCompany.id,
             startingLocation: this.startingLocation,
             returningLocation: this.returningLocation,
             startingDate: this.startingDate.toDateString(),
             returningDate: this.returningDate.toDateString(),
             totalDays: days,
             vehicleManufecter: vehicle.manufacturer,
             vehicleModel : vehicle.model,
             pricePerDay: vehicle.price,
             photo: vehicle.photo,
             vehicleid: vehicle.id,
             totalPrice,
             discount,
             different}
    });
  }

  loadParametres() {
    this.seats.two = true;
    this.seats.five = true;
    this.seats.six = true;

    this.doors.two = true;
    this.doors.four = true;
    this.doors.five = true;

    this.cartype.small = true;
    this.cartype.medium = true;
    this.cartype.large = true;
    this.cartype.luxury = true;

    this.averageRating.seven = true;
    this.averageRating.eight = true;
    this.averageRating.nine = true;
    this.averageRating.ten = true;

    this.vehicleParams.minPrice = 0;
    this.vehicleParams.averageRating = 0;
    this.vehicleParams.maxPrice = 400;
    this.vehicleParams.minSeats = 1;
    this.vehicleParams.maxSeats = 8;
    this.vehicleParams.minDoors = 2;
    this.vehicleParams.maxDoors = 7;
    this.vehicleParams.type = '';
    this.vehicleParams.pickupLocation = '';
    this.vehicleParams.startingDate = '';
    this.vehicleParams.returningDate = '';
  }

  onAddVehicle() {
    const dialogRef = this.dialog.open(AddVehicleDialogComponent, {
      width: '950px',
      height: '655px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.rentalService.getVehiclesForCompany(this.rentalCompany.id, 
        this.pagination.currentPage, this.pagination.itemsPerPage)
        .subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
        this.pagination = res.pagination;
      });
    });
  }

  onEditVehicle(vehicle: Vehicle) {
    const dialogRef = this.dialog.open(EditCarDialogComponent, {
      width: '400px',
      height: '655px',
      data: {...vehicle}
    });

    dialogRef.afterClosed().subscribe(result => {
     this.rentalService.editVehicle(result).subscribe(res => {
       this.alertify.success('Vehicle edited successfully!');
       this.rentalService.getVehiclesForCompany(this.rentalCompany.id).subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
        this.pagination = res.pagination;
      });
     }, error => {
      this.alertify.error('Failed to edit vehicle.');
     });
    });
  }

  onRemoveVehicle(vehicle: Vehicle) {
    this.alertify.confirm('Are you sure you want to remove vehicle? This action cannot be undone!', () => {
      this.rentalService.removeVehicle(vehicle.id).subscribe(res => {
        this.alertify.success('Vehicle successfuly deleted!');
        this.rentalService.getVehiclesForCompany(this.rentalCompany.id).subscribe((res: PaginatedResult<Vehicle[]>) => {
          this.vehicles = res.result;
          this.pagination = res.pagination;
        });
      }, error => {
        this.alertify.error('Failed to remove vehilce');
      });
    });
  }

  onCompanyIncomes() {
    const dialogRef = this.dialog.open(SelectDatesDialogComponent, {
      width: '450px',
      height: '350px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.rentalService.getIncomeStats(this.rentalCompany.id, result.startingDate, result.finalDate).subscribe(res => {
        this.dialog.open(CompanyIncomesDialogComponent, {
          width: '900px',
          height: '555px',
          data: {...res}
        });
      });
   });
  }

  onVehicleReservations() {
    this.rentalService.getReservationsStats(this.rentalCompany.id).subscribe(res => {
      this.companyResStats = res;
      this.dialog.open(CompanyReservationsDialogComponent, {
        width: '900px',
        height: '555px',
        data: {res}
      });
    }, error => {
      this.alertify.error('Error while loading stats!');
    });
  }

  onDiscountedVehicles() {
    this.dialog.open(VehiclesOnDiscountDialogComponent, {
      width: '850px',
      height: '770px',
      data: {id: this.rentalCompany.id}
    });
  }

  nextPage() { 
    this.route.params.subscribe(res => {
      this.rentalService.getVehiclesForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.vehicleParams)
      .subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
        this.pagination = res.pagination;
      }, error => {
        this.alertify.error('Failed to load vehicles!');
      });
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.nextPage();
  }

  onChangeHeadOffice() {
    const dialogRef = this.dialog.open(ChangeHeadofficeDialogComponent, {
      width: '450px',
      height: '350px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(_ => {
      this.loadCompany();
   });
  }

  OnRemoveDestinations() {
    const dialogRef = this.dialog.open(RemoveDestinationsDialogComponent, {
      width: '450px',
      height: '350px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(_ => {
      this.loadCompany();
   });
  }
}
