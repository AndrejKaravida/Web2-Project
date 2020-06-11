import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AvioService } from 'src/app/_services/avio.service';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { Router } from '@angular/router';
import { RentacaroptiondialogComponent } from '../rentacaroptiondialog/rentacaroptiondialog.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/custom-validators';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.component.html',
  styleUrls: ['./reservation-dialog.component.css']
})
export class ReservationDialogComponent implements OnInit {

  seatsLayout = {
    totalRows: 10,
    seatsPerRow: 6,
    seatsNaming: 'rowType',
    booked: ['1A', '5D']
  };
  seat: any;
  id: number;
  passports: 0;

  form: FormGroup = new FormGroup({});

  constructor(public dialogRef: MatDialogRef<ReservationDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private alertify: AlertifyService, private avioService: AvioService,
              private rentalService: CarrentalService, private router: Router,
              private dialog: MatDialog, private fb: FormBuilder) { }

  ngOnInit() {
    this.filterDestination();

    this.form=this.fb.group({
      passport: [this.passports, [CustomValidators.numberValidator]]
      
    });
  }

  Reserve() {
    const authId = localStorage.getItem('authId');
    const discount = this.data.discount;
    const price = discount ? this.data.flight.ticketPrice * 0.5 : this.data.flight.ticketPrice;
    this.avioService.makeFlightReservation(authId, this.data.flight.departureTime, this.data.flight.arrivalTime,
         this.data.flight.departureDestination.city, this.data.flight.arrivalDestination.city,
         price, this.data.flight.travelTime, this.data.company.id, this.data.company.name,
         this.data.company.photo, this.data.flight.id).subscribe(_ => {
          const dialogRef = this.dialog.open(RentacaroptiondialogComponent, {
            width: '450px',
            height: '350px',
            data: {id: this.id, arrivalTime: this.data.flight.arrivalTime, arrivalDestination:
              this.data.flight.arrivalDestination.city}
          });
          this.alertify.success('You have successfully booked this flight');
        });
  }

  getSelected(event) {
    this.seat = event;
  }

  filterDestination() {
    this.rentalService.getAllCarCompanies().subscribe(res => {
      res.forEach(element => {
        element.branches.forEach(branch => {
          if (branch.city === this.data.flight.arrivalDestination.city) {
            this.id = element.id;
          }
        });
      });
    });
  }

}
