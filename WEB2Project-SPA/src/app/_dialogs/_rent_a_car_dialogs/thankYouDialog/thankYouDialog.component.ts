import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';

@Component({
  selector: 'app-thankYouDialog',
  templateUrl: './thankYouDialog.component.html',
  styleUrls: ['./thankYouDialog.component.css']
})
export class ThankYouDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<ThankYouDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private router: Router) {}

  onClose(): void {
    this.router.navigate(['/myreservations']);
    this.dialogRef.close();
  }

}
