import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-thankYouDialog',
  templateUrl: './thankYouDialog.component.html',
  styleUrls: ['./thankYouDialog.component.css']
})
export class ThankYouDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ThankYouDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}
