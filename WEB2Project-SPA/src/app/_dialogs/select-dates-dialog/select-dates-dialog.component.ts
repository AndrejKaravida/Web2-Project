import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { CarCompanyIncomeStats } from 'src/app/_models/carcompanyincomestats';

@Component({
  selector: 'app-select-dates-dialog',
  templateUrl: './select-dates-dialog.component.html',
  styleUrls: ['./select-dates-dialog.component.css']
})
export class SelectDatesDialogComponent implements OnInit {
  startingDate = new Date();
  finalDate = new Date();

  constructor(
    public dialogRef: MatDialogRef<SelectDatesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private dialog: MatDialog) {}

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
