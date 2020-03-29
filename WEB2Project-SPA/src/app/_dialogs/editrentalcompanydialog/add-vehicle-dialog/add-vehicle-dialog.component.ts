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
  
  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {

    this.firstFormGroup = this.formBuilder.group({
      seats: ['', [Validators.min(1), Validators.required]],
      doors: ['', [Validators.min(1), Validators.required]],
    });

    this.secondFormGroup = this.formBuilder.group({
      manufacturer: ['', [Validators.minLength(2), Validators.required]],
      model: ['', [Validators.minLength(2), Validators.required]],
    });
  }

  onFileSelected() { 
    
  }

}
