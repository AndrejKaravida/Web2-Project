import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AvioService } from 'src/app/_services/avio.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css']
})
export class ReservationDialogComponent {

  seatsLayout = {
    totalRows: 10,
    seatsPerRow: 6,
    seatsNaming: 'rowType',
    booked: ['1A', '5D']
  };
  seat: any;

  constructor(public dialogRef: MatDialogRef<ReservationDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private alertify: AlertifyService, private avioService: AvioService) { }

  Reserve() {
    const authId = localStorage.getItem('authId');
    this.avioService.makeFlightReservation(authId, this.data.flight.departureTime, this.data.flight.arrivalTime,
         this.data.flight.departureDestination.city, this.data.flight.arrivalDestination.city,
         this.data.flight.ticketPrice, this.data.flight.travelTime, this.data.company.id, this.data.company.name,
         this.data.company.photo, this.data.flight.id).subscribe(_ => {
           this.alertify.success('You have successfully booked this flight');
        });
  }

  getSelected(event) {
    this.seat = event;
  }


}
