import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ThankYouDialogComponent } from '../thankYouDialog/thankYouDialog.component';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-viewCarDealDialog',
  templateUrl: './viewCarDealDialog.component.html',
  styleUrls: ['./viewCarDealDialog.component.css']
})
export class ViewCarDealDialogComponent{

  constructor(
    public dialogRef: MatDialogRef<ViewCarDealDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog,
    private rentalService: CarrentalService, private authService: AuthService) {}

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

    this.authService.userProfile$.subscribe(res => {
      this.rentalService.makeReservation(vehicleid, res.name, startDate, endDate, this.data.totalDays,
      this.data.totalPrice, this.data.companyName, this.data.companyId, this.data.returningLocation).subscribe(result => {
          this.dialog.open(ThankYouDialogComponent, {
            width: '600px',
            height: '350px',
            data: {...this.data}
            });
      });
    });
  }

}
