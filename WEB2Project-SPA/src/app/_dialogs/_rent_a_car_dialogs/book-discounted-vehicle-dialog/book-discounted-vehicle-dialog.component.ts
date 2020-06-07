import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { ThankYouDialogComponent } from '../thankYouDialog/thankYouDialog.component';

@Component({
  selector: 'app-book-discounted-vehicle-dialog',
  templateUrl: './book-discounted-vehicle-dialog.component.html',
  styleUrls: ['./book-discounted-vehicle-dialog.component.css']
})
export class BookDiscountedVehicleDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<BookDiscountedVehicleDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog,
    private rentalService: CarrentalService) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  onMakeReservation() {
    // tslint:disable: variable-name
    const starting = new Date(this.data.startingDate);
    const curr_date_st = starting.getDate();
    const curr_month_st = starting.getMonth() + 1;
    const curr_year_st = starting.getFullYear();

    const startDate = curr_date_st + '-' + curr_month_st + '-' + curr_year_st;

    const ending = new Date(this.data.returningDate);

    const curr_date_en = ending.getDate();
    const curr_month_en = ending.getMonth() + 1;
    const curr_year_en = ending.getFullYear();

    const endDate = curr_date_en + '-' + curr_month_en + '-' + curr_year_en;

    const vehicleid = this.data.vehicleid;

    const authId = localStorage.getItem('authId');
    this.rentalService.makeReservation(vehicleid, authId, startDate, endDate, this.data.totalDays,
      this.data.totalPrice, this.data.companyName, this.data.companyId, this.data.startingLocation,
      this.data.returningLocation).subscribe(result => {
          this.dialog.open(ThankYouDialogComponent, {
            width: '600px',
            height: '350px',
            data: {...this.data}
            });
      }, error => {
        if (error.error === 'Concurency error') {
          // tslint:disable-next-line: max-line-length
          alert ('Sorry but this car has been reserved by another user after you got the original value. The reservation has been canceled.');
        }
      });
    this.dialogRef.close();
  }
}
