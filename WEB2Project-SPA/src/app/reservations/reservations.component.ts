import { Component, OnInit, ViewChild } from '@angular/core';
import { Reservation } from '../_models/_carModels/carreservation';
import { CarrentalService } from '../_services/carrental.service';
import { Vehicle } from '../_models/_carModels/vehicle';
import { MatDialog } from '@angular/material/dialog';
import { RateVehicleDialogComponent } from '../_dialogs/_rent_a_car_dialogs/rate-vehicle-dialog/rate-vehicle-dialog.component';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { AvioService } from '../_services/avio.service';
import { FlightReservation } from '../_models/_avioModels/flightReservation';
import { RateflightdialogComponent } from '../_dialogs/_avio_company_dialogs/rateflightdialog/rateflightdialog.component';
import { Flight } from '../_models/_avioModels/flight';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  displayedColumns: string[] = ['#', 'image', 'model', 'totalDays', 'daysLeft', 'startDate', 'endDate', 'totalPrice', 'status', 'rate'];
  displayedColumns2: string[] = ['#', 'departuredest', 'depdate', 'arrivaldest', 'arrivaldate', 'price', 'status', 'rate' ];

  dataSource: MatTableDataSource<Reservation>;
  dataSource2: MatTableDataSource<FlightReservation>;

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild('secondPaginator') paginator2: MatPaginator;

  dateToday = new Date();

  constructor(private rentalService: CarrentalService, private dialog: MatDialog,
              private avioService: AvioService) { }

  ngOnInit() {
    this.loadReservations();
  }

  loadReservations() {
    const authId = localStorage.getItem('authId');
    this.rentalService.getCarReservationsForUser(authId).subscribe(response => {
      this.dataSource = new MatTableDataSource(response);
      this.dataSource.paginator = this.paginator;
    });
    this.avioService.getFlightReservationsForUser(authId).subscribe(result => {
      this.dataSource2 = new MatTableDataSource(result);
      this.dataSource2.paginator = this.paginator2;
    });
  }

  onRate(vehicle: Vehicle, companyName: string, companyId: string, reservationId: number) {
   const dialogRef =  this.dialog.open(RateVehicleDialogComponent, {
      width: '400px',
      height: '670px',
      data: {vehicle, companyName, companyId, reservationId}
    });
   dialogRef.afterClosed().subscribe(result => {
      this.loadReservations();
    });
  }

  onRateFlight(flight: Flight, companyName: string, companyId: string, companyPhoto: string, reservationId: number) {
    const dialogRef =  this.dialog.open(RateflightdialogComponent, {
      width: '400px',
      height: '670px',
      data: {flight, companyName, companyId, companyPhoto, reservationId}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadReservations();
    });
  }

  compare(date: Date) {
    if (new Date(date) < this.dateToday) {
      return true;
    } else {
      return false;
    }
  }
}
