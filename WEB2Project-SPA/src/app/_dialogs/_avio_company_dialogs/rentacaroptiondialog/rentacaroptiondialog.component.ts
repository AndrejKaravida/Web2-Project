import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { HowManyDaysDialogComponent } from '../../_rent_a_car_dialogs/how-many-days-dialog/how-many-days-dialog.component';

@Component({
  selector: 'app-rentacaroptiondialog',
  templateUrl: './rentacaroptiondialog.component.html',
  styleUrls: ['./rentacaroptiondialog.component.css']
})
export class RentacaroptiondialogComponent {

  constructor(public dialogRef: MatDialogRef<RentacaroptiondialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private dialog: MatDialog) { }

  routeToRentaACar() {
    this.dialog.open(HowManyDaysDialogComponent, {
      width: '500px',
      height: '400px',
      data: {registered: true, id: this.data.id,
        arrivalTime: this.data.arrivalTime, arrivalDestination: this.data.arrivalDestination}
    });
    this.dialogRef.close();
  }

  close() {
    this.dialogRef.close();
  }

}
