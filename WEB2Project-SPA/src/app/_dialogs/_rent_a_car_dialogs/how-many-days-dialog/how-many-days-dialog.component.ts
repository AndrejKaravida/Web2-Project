import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-how-many-days-dialog',
  templateUrl: './how-many-days-dialog.component.html',
  styleUrls: ['./how-many-days-dialog.component.css']
})
export class HowManyDaysDialogComponent {
  numberOfDays: 0;

  constructor(
    public dialogRef: MatDialogRef<HowManyDaysDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private router: Router) {}

    onNoClick(): void {
    this.dialogRef.close();
  }

  onConfirm() {
    this.router.navigate(['rentalprofile', this.data.id], {state: {data: {registered: true,
      arrivalTime: this.data.arrivalTime, arrivalDestination: this.data.arrivalDestination, 
      numberOfDays: this.numberOfDays}}});
    this.dialogRef.close();
  }
}
