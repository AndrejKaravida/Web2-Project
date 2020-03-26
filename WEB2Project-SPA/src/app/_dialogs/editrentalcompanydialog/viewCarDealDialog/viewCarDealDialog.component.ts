import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ThankYouDialogComponent } from '../thankYouDialog/thankYouDialog.component';

@Component({
  selector: 'app-viewCarDealDialog',
  templateUrl: './viewCarDealDialog.component.html',
  styleUrls: ['./viewCarDealDialog.component.css']
})
export class ViewCarDealDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ViewCarDealDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog) {}


  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onMakeReservation() { 
    const dialogRef = this.dialog.open(ThankYouDialogComponent, {
      width: '500px',
      height: '250px',
      data: {...this.data}
    });

    dialogRef.afterClosed().subscribe(result => {
    
   });
  }

}
