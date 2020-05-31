import { Component, OnInit, Input } from '@angular/core';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { MatDialog } from '@angular/material/dialog';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { ReservationDialogComponent } from 'src/app/_dialogs/_avio_company_dialogs/reservation-dialog/reservation-dialog.component';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AvioService } from 'src/app/_services/avio.service';

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
 
  constructor(private dialog: MatDialog,private alertify: AlertifyService, private avioService: AvioService) { }

  ngOnInit() {
  }

  ReserveFlight()
  {
    if (this.company == null || this.company == undefined) { 
      this.avioService.getCompanyForFlight(this.flight.id).subscribe(res => { 
        this.dialog.open(ReservationDialogComponent, {
          width: '800px',
          height: '1200px',
          data: {flight: this.flight, company: res}
        });
    
      });
    } else { 
      this.dialog.open(ReservationDialogComponent, {
        width: '800px',
        height: '1200px',
        data: {flight: this.flight, company: this.company}
      });
  
    }
 
  }

}
