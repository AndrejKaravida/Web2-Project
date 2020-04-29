import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Flight } from 'src/app/_models/flight';
import { User } from 'src/app/_models/_userModels/user';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AvioService } from 'src/app/_services/avio.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css']
})
export class ReservationDialogComponent implements OnInit {
  user: any;
  seatsLayout = {
    totalRows: 10,
    seatsPerRow: 6,
    seatsNaming: 'rowType',
    booked: ['1A', '5D']
  };
  seat: any;

  constructor(public dialogRef: MatDialogRef<ReservationDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Flight, private authService: AuthService,
              private alertify: AlertifyService, private avioService: AvioService) { }

  ngOnInit() {
    this.authService.userProfile$.subscribe(res => {
      this.user = res;
    });
  }

  Reserve() {
    this.authService.userProfile$.subscribe(res => {

      this.avioService.makeFlightReservation(res.email, res.nickname, this.data.departureTime, this.data.arrivalTime,
        this.data.departureDestination.city, this.data.arrivalDestination.city, this.data.ticketPrice, 123, "fdff")
        this.alertify.success('You have successfully booked this flight');
    });
  }

  getSelected(event) {
    this.seat = event;
  }


}