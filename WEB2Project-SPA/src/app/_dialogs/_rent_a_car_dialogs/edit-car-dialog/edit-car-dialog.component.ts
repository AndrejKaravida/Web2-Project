import { Component, Inject } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-edit-car-dialog',
  templateUrl: './edit-car-dialog.component.html',
  styleUrls: ['./edit-car-dialog.component.css']
})
export class EditCarDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<EditCarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Vehicle) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

}
