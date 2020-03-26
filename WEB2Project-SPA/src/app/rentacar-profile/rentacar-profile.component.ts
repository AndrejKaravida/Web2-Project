import { Component, OnInit } from '@angular/core';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/vehicle';
import { ActivatedRoute, Router } from '@angular/router';
import { EditrentalcompanydialogComponent } from '../_dialogs/editrentalcompanydialog/editrentalcompanydialog.component';
import { CarCompany } from '../_models/carcompany';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from '../_services/alertify.service';

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

  constructor(private rentalService: CarrentalService, private route: ActivatedRoute,
              private dialog: MatDialog, private alertify: AlertifyService) { }

  ngOnInit() {
    this.loadCompany();
    this.route.data.subscribe(data => {
      const key = 'vehicles';
      this.vehicles = data[key];
    });
    this.loadParametres();
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
        console.log(this.vehicleParams);
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

  openDialog(): void {
    const dialogRef = this.dialog.open(EditrentalcompanydialogComponent, {
      width: '400px',
      height: '630px',
      data: {...this.rentalCompany}
    });

    dialogRef.afterClosed().subscribe(result => {
      result.weekRentalDiscount = +result.weekRentalDiscount;
      result.monthRentalDiscount = +result.monthRentalDiscount;
      this.rentalService.updateComapny(result).subscribe(res => {
        this.alertify.success('Successfully changed company data!');
        this.loadCompany();
      });
      }, err => {
        this.alertify.error('Problem editing company data!');
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

}
