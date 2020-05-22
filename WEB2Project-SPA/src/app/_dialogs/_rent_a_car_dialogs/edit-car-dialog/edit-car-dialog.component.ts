import { Component, Inject, OnInit } from '@angular/core';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/custom-validators';

@Component({
  selector: 'app-edit-car-dialog',
  templateUrl: './edit-car-dialog.component.html',
  styleUrls: ['./edit-car-dialog.component.css']
})
export class EditCarDialogComponent implements OnInit {
 formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<EditCarDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Vehicle, private formBuilder: FormBuilder) {}

  ngOnInit() {
     this.formGroup = this.formBuilder.group({
      seats: [this.data.seats, [Validators.min(1), Validators.max(7),  Validators.required, CustomValidators.numberValidator]],
      doors: [this.data.doors.toString(), [Validators.min(1), Validators.max(7), Validators.required, CustomValidators.numberValidator]],
      manufacturer: [this.data.manufacturer, [Validators.minLength(2), Validators.required]],
      model: [this.data.model, [Validators.minLength(2), Validators.required]],
      type: [this.data.type.toString(), [Validators.required]]
      });
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

  editVehicle() {
    this.data.seats = this.formGroup.get('seats').value;
    this.data.doors = this.formGroup.get('doors').value;
    this.data.manufacturer = this.formGroup.get('manufacturer').value;
    this.data.model = this.formGroup.get('model').value;
    this.data.type = this.formGroup.get('type').value;
    this.dialogRef.close(this.data);
  }

}
