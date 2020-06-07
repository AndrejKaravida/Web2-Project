import { Component, OnInit, Inject } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { DiscountedVehicleParams } from 'src/app/_models/_carModels/discountedVehicleParams';

@Component({
  selector: 'app-discounted-vehicle-choose-dialog',
  templateUrl: './discounted-vehicle-choose-dialog.component.html',
  styleUrls: ['./discounted-vehicle-choose-dialog.component.css']
})
export class DiscountedVehicleChooseDialogComponent implements OnInit {

  vehiclesOnDiscount: Vehicle[];

  constructor( public dialogRef: MatDialogRef<DiscountedVehicleChooseDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
               private alertify: AlertifyService) { }

  ngOnInit() {
    const params: DiscountedVehicleParams = {
      pickupLocation: this.data.arrivalDestination,
      startingDate: this.data.startingDate,
      numberOfDays: this.data.numberOfDays
    };

    console.log(this.data);
    this.rentalService.getDiscountedVehiclesForUser(this.data.id, params).subscribe(res => {
      this.vehiclesOnDiscount = res;
    }, error => {
      this.alertify.error('Failed to load vehicles on discount!');
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
