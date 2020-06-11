import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { AvioService } from 'src/app/_services/avio.service';
import { FlightToMake } from 'src/app/_models/_avioModels/flightToMake';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { Destination } from 'src/app/_models/_avioModels/destination';
import { FormControl, Validators, FormGroup, FormBuilder, AbstractControl } from '@angular/forms';
import { CustomValidators } from 'src/app/custom-validators';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-edit-flight-dialog',
  templateUrl: './edit-flight-dialog.component.html',
  styleUrls: ['./edit-flight-dialog.component.css']
})
export class EditFlightDialogComponent implements OnInit {
  newFlight: FlightToMake = {
    departureDestination: '',
    arrivalDestination: '',
    averageGrade: 0,
    departureTime: new Date(),
    arrivalTime: new Date(),
    discount: false,
    travelLength: 0,
    travelDuration: 0,
    price: 0,
    luggage: 0
  };
  destinations: Destination[];
  flight: Flight;
  startingMinDate = new Date();
 form: FormGroup = new FormGroup({});
 isEdit = false;

  constructor(public dialogRef: MatDialogRef<EditFlightDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private avioService: AvioService, private fb: FormBuilder,
                private alertify: AlertifyService) { }

  ngOnInit() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.newFlight.departureDestination = this.destinations[0].city;
      this.newFlight.arrivalDestination = this.destinations[1].city;
    });

    if(!this.data.boolEdit) { 
      this.form=this.fb.group({
        luggages: ['', [Validators.min(14), Validators.max(70),  Validators.required, CustomValidators.numberValidator]],
        travelDurations:['',[CustomValidators.numberValidator]],
        prices: ['', [CustomValidators.numberValidator]],
        distances: ['', [CustomValidators.numberValidator]]
      });
    }
    else { 
      this.form=this.fb.group({
        // tslint:disable-next-line: max-line-length
        luggages: [this.data.flightForSend.luggage, [Validators.min(14), Validators.max(70),  Validators.required, CustomValidators.numberValidator]],
        travelDurations :[this.data.flightForSend.travelTime,[CustomValidators.numberValidator]],
        prices: [this.data.flightForSend.ticketPrice, [CustomValidators.numberValidator]],
        distances: [this.data.flightForSend.mileage, [CustomValidators.numberValidator]]
      });
    }

    this.isEdit = this.data.boolEdit;
  }

  editFlight() { 

    this.data.flightForSend.luggage = this.form.get('luggages').value;
    this.data.flightForSend.mileage = this.form.get('distances').value;
    this.data.flightForSend.ticketPrice = this.form.get('prices').value;
    this.data.flightForSend.travelTime = this.form.get('travelDurations').value;
    this.data.flightForSend.companyId = this.data.id;

    this.avioService.editFlight(this.data.flightForSend.id, this.data.flightForSend).subscribe(res => {
      this.alertify.success('You have successfuly changed this flight!');
      this.dialogRef.close();
    });
  
  }

  SaveFlight(){

    this.newFlight.luggage = this.form.get('luggages').value;
    this.newFlight.travelLength = this.form.get('distances').value;
    this.newFlight.price = this.form.get('prices').value;
    this.newFlight.travelDuration = this.form.get('travelDurations').value;

    this.avioService.makeNewFlight(this.data.id, this.newFlight).subscribe(res => {
      this.dialogRef.close();
    });

   
    
    
  }

}

