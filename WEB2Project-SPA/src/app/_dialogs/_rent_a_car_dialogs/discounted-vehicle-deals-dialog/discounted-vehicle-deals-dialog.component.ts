import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { DiscountedVehicleChooseDialogComponent } from '../discounted-vehicle-choose-dialog/discounted-vehicle-choose-dialog.component';

@Component({
  selector: 'app-discounted-vehicle-deals-dialog',
  templateUrl: './discounted-vehicle-deals-dialog.component.html',
  styleUrls: ['./discounted-vehicle-deals-dialog.component.css']
})
export class DiscountedVehicleDealsDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<DiscountedVehicleDealsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dialog: MatDialog) {}

    onNoClick(): void {
    this.dialogRef.close();
  }

  onConfirm() {
    this.dialog.open(DiscountedVehicleChooseDialogComponent, {
      width: '850px',
      height: '770px',
      data: {id: this.data.id, startingDate: this.data.arrivalTime, pickupLocation: this.data.arrivalDestination,
        numberOfDays: this.data.numberOfDays}
    });
    this.dialogRef.close();
  }
}
