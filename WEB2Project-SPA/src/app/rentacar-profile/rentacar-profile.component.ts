// tslint:disable: max-line-length
import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/_carModels/vehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { EditrentalcompanydialogComponent } from '../_dialogs/_rent_a_car_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/_carModels/carcompany';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';
import { ViewCarDealDialogComponent } from '../_dialogs/_rent_a_car_dialogs/viewCarDealDialog/viewCarDealDialog.component';
import { AddVehicleDialogComponent } from '../_dialogs/_rent_a_car_dialogs/add-vehicle-dialog/add-vehicle-dialog.component';
import { EditCarDialogComponent } from '../_dialogs/_rent_a_car_dialogs/edit-car-dialog/edit-car-dialog.component';
import { CompanyIncomesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/company-incomes-dialog/company-incomes-dialog.component';
import { CompanyReservationsDialogComponent } from '../_dialogs/_rent_a_car_dialogs/company-reservations-dialog/company-reservations-dialog.component';
import { CarCompanyReservationStats } from '../_models/_carModels/carcompanyresstats';
import { SelectDatesDialogComponent } from '../_dialogs/_rent_a_car_dialogs/select-dates-dialog/select-dates-dialog.component';
import { VehiclesOnDiscountDialogComponent } from '../_dialogs/_rent_a_car_dialogs/vehicles-on-discount-dialog/vehicles-on-discount-dialog.component';
import { ShowMapDialogComponent } from '../_dialogs/_rent_a_car_dialogs/show-map-dialog/show-map-dialog.component';
import { Pagination, PaginatedResult } from '../_models/_shared/pagination';
import { ChangeHeadofficeDialogComponent } from '../_dialogs/_rent_a_car_dialogs/change-headoffice-dialog/change-headoffice-dialog.component';
import { RemoveDestinationsDialogComponent } from '../_dialogs/_rent_a_car_dialogs/remove-destinations-dialog/remove-destinations-dialog.component';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';
import { AddNewBranchDialogComponent } from '../_dialogs/_rent_a_car_dialogs/add-new-branch-dialog/add-new-branch-dialog.component';
import { ChangeVehicleLocationDialogComponent } from '../_dialogs/_rent_a_car_dialogs/changeVehicleLocationDialog/changeVehicleLocationDialog.component';
import { DiscountedVehicleDealsDialogComponent } from '../_dialogs/_rent_a_car_dialogs/discounted-vehicle-deals-dialog/discounted-vehicle-deals-dialog.component';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit {

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog, private alertify: AlertifyService,
              private store: Store<fromRoot.State>, private router: Router) { }
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
  role$: Observable<string>;
  isAdmin = false;

  ngOnInit() {
    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.role$ = this.store.select(fromRoot.getRole);

    this.route.data.subscribe(data => {
      this.vehicles = data.vehicles.result;
      this.rentalCompany = data.carcompany;
      this.pagination = data.vehicles.pagination;
    });
    this.loadParametres();
    this.startingLocation = this.rentalCompany.headOffice.city;
    this.returningLocation = this.rentalCompany.headOffice.city;
    this.role$.subscribe(res => {
      if ((res === 'managerCarNo' + this.rentalCompany.id) || res === 'sysadmin') {
        this.isAdmin = true;
      }
    });

    if (history.state.data?.registered) {
      this.dialog.open(DiscountedVehicleDealsDialogComponent, {
        width: '500px',
        height: '300px',
        data: {id: this.rentalCompany.id, arrivalTime: history.state.data.arrivalTime,
          arrivalDestination: history.state.data.arrivalDestination, numberOfDays: history.state.data.numberOfDays}
      });
    }
  }

  onShowMap(mapString: string) {
    this.dialog.open(ShowMapDialogComponent, {
      width: '1200px',
      height: '800px',
      data: {mapString}
    });
  }

  loadVehicles() {
    if (this.startingDate == null || this.returningDate == null) {
      this.disabled = true;
      return;
    }
    this.disabled = false;

    this.vehicleParams.averageRating = this.averageRating;
    this.vehicleParams.cartype = this.cartype;
    this.vehicleParams.doors = this.doors;
    this.vehicleParams.seats = this.seats;

    this.vehicleParams.pickupLocation = this.startingLocation;
    this.vehicleParams.startingDate = this.startingDate.toLocaleDateString();
    this.vehicleParams.returningDate = this.returningDate.toLocaleDateString();

    this.route.params.subscribe(res => {
      // tslint:disable: no-shadowed-variable
      this.rentalService.getVehiclesForCompany(res.id, this.pagination.currentPage,
        this.pagination.itemsPerPage, this.vehicleParams).subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
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
      height: '720px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {

        result.weekRentalDiscount = +result.weekRentalDiscount;
        result.monthRentalDiscount = +result.monthRentalDiscount;
        this.rentalService.updateComapny(result).subscribe(res => {
          this.alertify.success('Successfully changed company data!');
          this.loadCompany();
        }, error => {
          if (error.error === 'Concurency error') {
            this.loadCompany();
            const errString = 'The record you attempted to edit was modified by another user after you got the original value. ' +
            'The edit operation was canceled and the current values in the database have been displayed. ' +
            'If you still want to edit this record, please open the dialog and make changed again.';
            alert (errString);
          }
        });
      }
      });
  }

  onAddNewDestination() {
    const dialogRef = this.dialog.open(AddNewBranchDialogComponent, {
      width: '500px',
      height: '750px',
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

    if (this.startingDate.getDate() === this.returningDate.getDate()) {
      this.disabled = true;
      this.alertify.warning('You have to choose at least 1 day!');
      return;
    }

    const diffc = this.returningDate.getTime() - this.startingDate.getTime();

    const days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));

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
    this.vehicleParams.maxPrice = 400;

    this.vehicleParams.pickupLocation = '';
    this.vehicleParams.startingDate = '';
    this.vehicleParams.returningDate = '';

    this.vehicleParams.averageRating = this.averageRating;
    this.vehicleParams.cartype = this.cartype;
    this.vehicleParams.doors = this.doors;
    this.vehicleParams.seats = this.seats;
  }

  onAddVehicle() {
    const dialogRef = this.dialog.open(AddVehicleDialogComponent, {
      width: '950px',
      height: '700px',
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
      height: '755px',
      data: {...vehicle}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.rentalService.editVehicle(result, this.rentalCompany.id).subscribe(res => {
          this.alertify.success('Vehicle edited successfully!');
          this.rentalService.getVehiclesForCompany(this.rentalCompany.id, this.pagination.currentPage, this.pagination.itemsPerPage).subscribe((res: PaginatedResult<Vehicle[]>) => {
           this.vehicles = res.result;
           this.pagination = res.pagination;
         });
        }, error => {
          let errorMessage = '';

          for (const err of error.error.errors) {
         errorMessage += err.message;
         errorMessage += '\n';
        }
          this.alertify.error(errorMessage);
        });
      }
    });
  }

  onRemoveVehicle(vehicle: Vehicle) {
    this.alertify.confirm('Are you sure you want to remove vehicle? This action cannot be undone!', () => {
      this.rentalService.removeVehicle(vehicle.id, this.rentalCompany.id).subscribe(res => {
        this.alertify.success('Vehicle successfuly deleted!');
        this.vehicles = this.vehicles.filter(x => x.id !== vehicle.id);
        this.pagination.totalItems = this.pagination.totalItems - 1;
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

  onCompanyIncomes() {
    const dialogRef = this.dialog.open(SelectDatesDialogComponent, {
      width: '450px',
      height: '350px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.rentalService.getIncomeStats(this.rentalCompany.id, result.startingDate.toLocaleDateString(), result.finalDate.toLocaleDateString()).subscribe(res => {
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
      let errorMessage = '';

      for (const err of error.error.errors) {
     errorMessage += err.message;
     errorMessage += '\n';
    }
      this.alertify.error(errorMessage);
    });
  }

  onDiscountedVehicles() {
    this.dialog.open(VehiclesOnDiscountDialogComponent, {
      width: '850px',
      height: '770px',
      data: {id: this.rentalCompany.id, rentalCompany: this.rentalCompany}
    });
  }

  nextPage() {
    this.route.params.subscribe(res => {
      this.rentalService.getVehiclesForCompany(res.id, this.pagination.currentPage, this.pagination.itemsPerPage, this.vehicleParams)
      .subscribe((res: PaginatedResult<Vehicle[]>) => {
        this.vehicles = res.result;
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

  onChangeVehicleLocation(vehicle: Vehicle) {
    const dialogRef = this.dialog.open(ChangeVehicleLocationDialogComponent, {
      width: '450px',
      height: '350px',
      data: {company: this.rentalCompany, vehicle}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.vehicles.find(x => x.id === vehicle.id).currentDestination = result;
      }
   });
  }
}


