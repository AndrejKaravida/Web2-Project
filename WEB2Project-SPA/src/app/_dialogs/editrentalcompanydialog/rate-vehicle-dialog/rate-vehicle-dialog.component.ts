import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ThankYouForRateDialogComponent } from '../thankYouForRateDialog/thankYouForRateDialog.component';

@Component({
  selector: 'app-rate-vehicle-dialog',
  templateUrl: './rate-vehicle-dialog.component.html',
  styleUrls: ['./rate-vehicle-dialog.component.css']
})
export class RateVehicleDialogComponent implements OnInit {
  selected : string;
  selected2: string;

  constructor(
    public dialogRef: MatDialogRef<RateVehicleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dialog: MatDialog) {}

  ngOnInit() {
  }

  onClose(): void {
    this.dialogRef.close();
    this.dialog.open(ThankYouForRateDialogComponent, {
      width: '500px',
      height: '300px',
      data: {companyName: this.data.companyName}
      });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
