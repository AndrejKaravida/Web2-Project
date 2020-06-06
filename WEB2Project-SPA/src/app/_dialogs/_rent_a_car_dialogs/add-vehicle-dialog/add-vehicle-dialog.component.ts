import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Vehicle } from 'src/app/_models/_carModels/vehicle';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { HttpClient } from '@angular/common/http';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { CustomValidators } from 'src/app/custom-validators';

@Component({
  selector: 'app-add-vehicle-dialog',
  templateUrl: './add-vehicle-dialog.component.html',
  styleUrls: ['./add-vehicle-dialog.component.css']
})
export class AddVehicleDialogComponent implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  thirdFormGroup: FormGroup;

  newVehicle: Vehicle = {
    id: 0,
    manufacturer: '',
    model: '',
    averageGrade: 0,
    doors: 0,
    currentDestination: '',
    seats: 0,
    price: 0,
    photo: '',
    type: '',
    oldPrice: 0
  };
  selectedFile = null;

  constructor(private formBuilder: FormBuilder, private rentalService: CarrentalService,
              private http: HttpClient, private alertify: AlertifyService,
              public dialogRef: MatDialogRef<AddVehicleDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {

    this.firstFormGroup = this.formBuilder.group({
      seats: ['', [Validators.min(1), Validators.required]],
      doors: ['', [Validators.min(1), Validators.required]],
    });

    this.secondFormGroup = this.formBuilder.group({
      manufacturer: ['', [Validators.minLength(2), Validators.maxLength(20), Validators.required]],
      model: ['', [Validators.minLength(2), Validators.maxLength(20), Validators.required]],
      type: ['', [Validators.required]],
    });

    this.thirdFormGroup = this.formBuilder.group({
      price: ['', [Validators.min(1), Validators.max(500), Validators.required, CustomValidators.numberValidator]],
      currentDestination: ['', Validators.required]
    });

  }

  onFileSelected(event) {
    this.selectedFile = event.target.files[0] as File;
  }

  addVehicle() {
    this.newVehicle.doors = this.firstFormGroup.get('doors').value;
    this.newVehicle.seats = this.firstFormGroup.get('seats').value;
    this.newVehicle.manufacturer = this.secondFormGroup.get('manufacturer').value;
    this.newVehicle.model = this.secondFormGroup.get('model').value;
    this.newVehicle.type = this.secondFormGroup.get('type').value;
    this.newVehicle.price = this.thirdFormGroup.get('price').value;
    this.newVehicle.currentDestination = this.thirdFormGroup.get('currentDestination').value;

    if (this.selectedFile == null || this.selectedFile === undefined) {
      alert('Please choose the photo!');
    } else {
      this.rentalService.addVehicle(this.newVehicle, this.data.id).subscribe((data: any) => {
        const fd = new FormData();
        fd.append('file', this.selectedFile, this.selectedFile.name);
        return this.http.post('http://localhost:5000/api/upload/' + data.id + '/' + this.data.id, fd)
        .subscribe(res => {
          this.alertify.success('Successfully added vehicle!');
          this.dialogRef.close();
        }, error => {
          let errorMessage = '';

          for (const err of error.error.errors) {
         errorMessage += err.message;
         errorMessage += '\n';
        }
          this.alertify.error(errorMessage);
        });
      });
    }

  }

}
