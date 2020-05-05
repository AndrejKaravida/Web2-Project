import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ThankYouForRateDialogComponent } from '../thankYouForRateDialog/thankYouForRateDialog.component';
import { CarrentalService } from 'src/app/_services/carrental.service';

@Component({
  selector: 'app-rate-vehicle-dialog',
  templateUrl: './rate-vehicle-dialog.component.html',
  styleUrls: ['./rate-vehicle-dialog.component.css']
})
export class RateVehicleDialogComponent {
  selected = '';
  selected2 = '';

  constructor(
    public dialogRef: MatDialogRef<RateVehicleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dialog: MatDialog,
    private rentalService: CarrentalService) {}

  onClose(): void {
    const userId = localStorage.getItem('authId');
    if(this.selected.length > 0 && this.selected2.length > 0) {
      this.rentalService.rate(this.data.vehicle.id, this.selected2, userId,
        this.data.reservationId, this.selected, this.data.companyId).subscribe(res => {
        this.dialogRef.close(true);
        this.dialog.open(ThankYouForRateDialogComponent, {
          width: '500px',
          height: '300px',
          data: {companyName: this.data.companyName}
        });
      });
    } else {
      alert('Please choose a rating first!');
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
