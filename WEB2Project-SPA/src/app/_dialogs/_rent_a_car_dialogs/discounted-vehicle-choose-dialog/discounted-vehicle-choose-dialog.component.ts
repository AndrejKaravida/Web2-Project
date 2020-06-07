import { Component, OnInit, Inject } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

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
    this.loadVehicles();
  }

  loadVehicles() {
    this.rentalService.getDiscountedVehiclesForCompany(this.data.id).subscribe(res => { 
      this.vehiclesOnDiscount = res;
    }, error => {
      this.alertify.error('Failed to load vehicles on discount!');
    });
  }

  onCancel(){
    this.dialogRef.close();
  }
}
