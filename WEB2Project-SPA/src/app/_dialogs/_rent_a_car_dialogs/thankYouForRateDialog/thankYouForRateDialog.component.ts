import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-thankYouForRateDialog',
  templateUrl: './thankYouForRateDialog.component.html',
  styleUrls: ['./thankYouForRateDialog.component.css']
})
export class ThankYouForRateDialogComponent  {

  constructor(
    public dialogRef: MatDialogRef<ThankYouForRateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  onClose(): void {
    this.dialogRef.close();
  }

}
