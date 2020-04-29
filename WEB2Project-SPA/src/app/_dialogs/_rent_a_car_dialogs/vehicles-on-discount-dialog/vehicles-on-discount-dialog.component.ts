import { Component, OnInit, Inject } from '@angular/core';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Vehicle } from 'src/app/_models/vehicle';

@Component({
  selector: 'app-vehicles-on-discount-dialog',
  templateUrl: './vehicles-on-discount-dialog.component.html',
  styleUrls: ['./vehicles-on-discount-dialog.component.css']
})
export class VehiclesOnDiscountDialogComponent implements OnInit {
  vehiclesOnDiscount: Vehicle[];

  constructor( public dialogRef: MatDialogRef<VehiclesOnDiscountDialogComponent>,
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
