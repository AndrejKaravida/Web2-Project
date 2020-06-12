import { Component, OnInit, Input } from '@angular/core';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { MatDialog } from '@angular/material/dialog';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { ReservationDialogComponent } from 'src/app/_dialogs/_avio_company_dialogs/reservation-dialog/reservation-dialog.component';
import { AvioService } from 'src/app/_services/avio.service';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import * as fromRoot from '../../app.reducer';
import { EditFlightDialogComponent } from 'src/app/_dialogs/_avio_company_dialogs/edit-flight-dialog/edit-flight-dialog.component';

@Component({
  selector: 'app-flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.css']
})
export class FlightCardComponent implements OnInit {
  faPlaneDeparture = faPlaneDeparture;
  faPlaneArrival = faPlaneArrival;
  faArrowRight = faArrowRight;
  @Input() flight: Flight;
  @Input() company: AvioCompany;

  isAuth$: Observable<boolean>;
  role$: Observable<string>;
  isAdmin = false;

  constructor(private dialog: MatDialog, private avioService: AvioService, private store: Store<fromRoot.State>) { }

  ngOnInit() {
    this.isAuth$ = this.store.select(fromRoot.getIsAuth);
    this.role$ = this.store.select(fromRoot.getRole);

    this.role$.subscribe(res => {
        if ((res === 'managerAvioNo' + this.company.id) || res === 'sysadmin') {
          this.isAdmin = true;
        }
      });
  }

  ReserveFlight() {
    if (this.company == null || this.company === undefined) {
      this.avioService.getCompanyForFlight(this.flight.id).subscribe(res => {
        this.dialog.open(ReservationDialogComponent, {
          width: '700px',
          height: '600px',
          data: {flight: this.flight, company: res, discount: false}
        });

      });
    } else {
      this.dialog.open(ReservationDialogComponent, {
        width: '700px',
        height: '600px',
        data: {flight: this.flight, company: this.company, discount: false}
      });

    }

  }
  EditFlight() {
    const dialogRef = this.dialog.open(EditFlightDialogComponent, {
      width: '550px',
      height: '850px',
      data: {id: this.company.id, flightForSend: this.flight, boolEdit: true}
    });


    dialogRef.afterClosed().subscribe(result => {
   });
  }

}
