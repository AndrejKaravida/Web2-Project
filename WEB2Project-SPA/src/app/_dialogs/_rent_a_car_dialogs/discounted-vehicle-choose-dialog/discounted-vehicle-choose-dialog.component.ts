import { Component, OnInit, Inject } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { DiscountedVehicleParams } from 'src/app/_models/_carModels/discountedVehicleParams';
import { CarCompany } from 'src/app/_models/_carModels/carcompany';
import { BookDiscountedVehicleDialogComponent } from '../book-discounted-vehicle-dialog/book-discounted-vehicle-dialog.component';

@Component({
  selector: 'app-discounted-vehicle-choose-dialog',
  templateUrl: './discounted-vehicle-choose-dialog.component.html',
  styleUrls: ['./discounted-vehicle-choose-dialog.component.css']
})
export class DiscountedVehicleChooseDialogComponent implements OnInit {

  vehiclesOnDiscount: Vehicle[];
  rentalCompany: CarCompany;
  chosenVehicle: Vehicle;

  constructor( public dialogRef: MatDialogRef<DiscountedVehicleChooseDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
               private alertify: AlertifyService, private dialog: MatDialog) { }

  ngOnInit() {
    const params: DiscountedVehicleParams = {
      pickupLocation: this.data.pickupLocation,
      startingDate: this.data.startingDate,
      numberOfDays: this.data.numberOfDays
    };

    this.rentalService.getCarRentalCompany(this.data.id).subscribe(res=> { 
      this.rentalCompany = res;
    });

    this.rentalService.getDiscountedVehiclesForUser(this.data.id, params).subscribe(res => {
      this.vehiclesOnDiscount = res;
    }, error => {
      this.alertify.error('Failed to load vehicles on discount!');
    });
  }

  onBookVehicle(vehicle: Vehicle) {
    const startingDate = new Date(this.data.startingDate);
    const returningDate = new Date(startingDate.getTime() + (1000*60*60*24*this.data.numberOfDays));

    const diffc = returningDate.getTime() - startingDate.getTime();

    const days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));

    let discount = 0;
    const different = false;

    if (days > 29) {
      discount = this.rentalCompany.monthRentalDiscount;
    } else if (days > 6) {
      discount = this.rentalCompany.weekRentalDiscount;
    }

    const totalPrice = days * vehicle.price;

    this.dialog.open(BookDiscountedVehicleDialogComponent, {
      width: '400px',
      height: '680px',
      data: {companyName: this.rentalCompany.name,
             companyId: this.rentalCompany.id,
             startingLocation: this.data.pickupLocation,
             returningLocation: this.data.pickupLocation,
             startingDate: startingDate.toDateString(),
             returningDate: returningDate.toDateString(),
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
    this.dialogRef.close();
  }

  onCancel() {
    this.dialogRef.close();
  }
}
