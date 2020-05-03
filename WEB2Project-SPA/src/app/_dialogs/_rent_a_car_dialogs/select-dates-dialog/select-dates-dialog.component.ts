import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-select-dates-dialog',
  templateUrl: './select-dates-dialog.component.html',
  styleUrls: ['./select-dates-dialog.component.css']
})
export class SelectDatesDialogComponent {
  startingDate = new Date();
  finalDate = new Date();

  constructor(
    public dialogRef: MatDialogRef<SelectDatesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

}
