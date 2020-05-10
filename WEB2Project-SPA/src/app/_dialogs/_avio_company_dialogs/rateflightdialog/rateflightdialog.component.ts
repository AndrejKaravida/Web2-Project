import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { AvioService } from 'src/app/_services/avio.service';
import { ThankYouForRateDialogComponent } from '../../_rent_a_car_dialogs/thankYouForRateDialog/thankYouForRateDialog.component';

@Component({
  selector: 'app-rateflightdialog',
  templateUrl: './rateflightdialog.component.html',
  styleUrls: ['./rateflightdialog.component.css']
})
export class RateflightdialogComponent {
  selected = '';
  selected2 = '';
  
  constructor(
    public dialogRef: MatDialogRef<RateflightdialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dialog: MatDialog,
    private avioService: AvioService) {}

    onClose(): void {
      const userId = localStorage.getItem('authId');
      if(this.selected.length > 0 && this.selected2.length > 0) {
        this.avioService.rate(this.data.flight.id, this.selected2, userId,
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
