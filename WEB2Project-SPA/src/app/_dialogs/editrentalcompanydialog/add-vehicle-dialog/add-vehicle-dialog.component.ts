import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Vehicle } from 'src/app/_models/vehicle';

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
    id: null,
    manufacturer: '',
    model: '',
    averageGrade: 0,
    doors: 0,
    seats: 0,
    price: 0,
    photo: '',
    type: ''
  };
  selectedFile = null;
  
  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {

    this.firstFormGroup = this.formBuilder.group({
      seats: ['', [Validators.min(1), Validators.required]],
      doors: ['', [Validators.min(1), Validators.required]],
    });

    this.secondFormGroup = this.formBuilder.group({
      manufacturer: ['', [Validators.minLength(2), Validators.required]],
      model: ['', [Validators.minLength(2), Validators.required]],
      type: ['', [Validators.required]],
    });

    this.thirdFormGroup = this.formBuilder.group({
      price: ['', [Validators.min(1), Validators.required]]
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

    if(this.selectedFile == null || this.selectedFile == undefined) { 
      alert('Please choose the photo!');
    }
    else { 
      console.log(this.newVehicle);
    }

  }

}
