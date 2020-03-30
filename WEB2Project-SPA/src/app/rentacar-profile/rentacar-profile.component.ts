import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { EditrentalcompanydialogComponent } from '../_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/carcompany';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';
import { FormControl } from '@angular/forms';
import { ViewCarDealDialogComponent } from '../_dialogs/editrentalcompanydialog/viewCarDealDialog/viewCarDealDialog.component';
import { AddVehicleDialogComponent } from '../_dialogs/editrentalcompanydialog/add-vehicle-dialog/add-vehicle-dialog.component';
import { EditCarDialogComponent } from '../_dialogs/editrentalcompanydialog/edit-car-dialog/edit-car-dialog.component';
import { CompanyIncomesDialogComponent } from '../_dialogs/editrentalcompanydialog/company-incomes-dialog/company-incomes-dialog.component';

@Component({
  selector: 'app-rentacar-profile',
  templateUrl: './rentacar-profile.component.html',
  styleUrls: ['./rentacar-profile.component.css']
})
export class RentacarProfileComponent implements OnInit { 
  rentalCompany: CarCompany;
  vehicles: Vehicle[];
  vehicleParams: any = {};
  averageRating: any = {};
  cartype: any = {};
  doors: any = {};
  seats: any = {};
  startingLocation = '';
  returningLocation = '';
  startingDate = new Date();
  returningDate = new Date();
  startingMinDate = new Date();
  returningMinDate = new Date();

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadCompany();
    this.route.data.subscribe(data => {
      const key = 'vehicles';
      this.vehicles = data[key];
    });
    this.loadParametres();
    this.startingLocation = this.rentalCompany.locations[1].address;
    this.returningLocation = this.rentalCompany.locations[1].address;
    this.returningMinDate.setDate(this.returningMinDate.getDate() + 1);
    this.returningDate.setDate(this.returningDate.getDate() + 7);
  }

  loadVehicles() {
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

    this.route.params.subscribe(res => {
      // tslint:disable-next-line: no-shadowed-variable
      this.rentalService.getVehiclesForCompany(res.id, this.vehicleParams).subscribe(res => {
        this.vehicles = res;
      }, error => {
        this.alertify.error('Failed to load vehicles!');
      })
    });
  }

  resetFilters() {
    this.route.data.subscribe(data => {
      const key = 'vehicles';
      this.vehicles = data[key];
    });
    this.loadParametres();
  }

  loadCompany() {
    this.route.data.subscribe(data => {
      const key = 'carcompany';
      this.rentalCompany = data[key];
    });
  }

  onEditCompany() {
    this.openDialog();
  }

  onViewDeal(vehicle: Vehicle) {
    this.openViewDialog(vehicle);
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(EditrentalcompanydialogComponent, {
      width: '400px',
      height: '630px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result) {

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

  openViewDialog(vehicle: Vehicle): void {   
    var diffc = this.returningDate.getTime() - this.startingDate.getTime();
   
    var days = Math.round(Math.abs(diffc/(1000*60*60*24)));

    let discount = 0;

    if (days > 29) {
      discount = this.rentalCompany.monthRentalDiscount;
    } else if (days > 6) {
      discount = this.rentalCompany.weekRentalDiscount;
    }

    let totalPrice = days * vehicle.price;

    if (discount > 0) {
      totalPrice = totalPrice - (totalPrice * (discount / 100));
    }


    const dialogRef = this.dialog.open(ViewCarDealDialogComponent, {
      width: '400px',
      height: '630px',
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
             discount}
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
    this.vehicleParams.minSeats = 1;
    this.vehicleParams.maxSeats = 8;
    this.vehicleParams.minDoors = 2;
    this.vehicleParams.maxDoors = 7;
    this.vehicleParams.type = '';
  }

  onAddVehicle() {
    const dialogRef = this.dialog.open(AddVehicleDialogComponent, {
      width: '950px',
      height: '655px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      this.rentalService.getVehiclesForCompanyNoParams(this.rentalCompany.id).subscribe(res => {
        this.vehicles = res;
        this.rentalCompany.vehicles = res;
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
       this.rentalService.getVehiclesForCompanyNoParams(this.rentalCompany.id).subscribe(result => {
        this.vehicles = result;
        this.rentalCompany.vehicles = result;
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
        this.rentalService.getVehiclesForCompanyNoParams(this.rentalCompany.id).subscribe(result => {
          this.vehicles = result;
          this.rentalCompany.vehicles = result;
        });
      }, error => {
        this.alertify.error('Failed to remove vehilce');
      });
    });
  }

  onCompanyIncomes() { 
    const dialogRef = this.dialog.open(CompanyIncomesDialogComponent, {
      width: '600px',
      height: '455px',
      data: {}
    });
  }

  onVehicleReservations() { 
    
  }

}
