import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Flight } from 'src/app/_models/flight';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css']
})
export class ReservationDialogComponent implements OnInit {
  user: any;

  constructor(public dialogRef: MatDialogRef<ReservationDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: Flight, private authService: AuthService, 
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.authService.userProfile$.subscribe(res => {
      this.user = res;
    });
  }

  ReserveAlertify(){ 
    this.alertify.success('You have successfully booked this flight');
  }
}
