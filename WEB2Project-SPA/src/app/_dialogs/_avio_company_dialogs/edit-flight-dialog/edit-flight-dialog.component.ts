import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Flight } from 'src/app/_models/flight';
import { AvioService } from 'src/app/_services/avio.service';
import { FlightToMake } from 'src/app/_models/flightToMake';
import { AvioCompany } from 'src/app/_models/aviocompany';
import { Destination } from 'src/app/_models/destination';

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
    price: 0
  };
  destinations: Destination[];
  startingMinDate = new Date();

  constructor(public dialogRef: MatDialogRef<EditFlightDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private avioService: AvioService) { }

  ngOnInit() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.destinations = res;
      this.newFlight.departureDestination = this.destinations[0].city;
      this.newFlight.arrivalDestination = this.destinations[1].city;
    });
  }

  SaveFlight(){
    console.log(this.newFlight);


    this.avioService.makeNewFlight(this.data.id, this.newFlight).subscribe(res => {
      console.log(res);
    });
    
  }

}
