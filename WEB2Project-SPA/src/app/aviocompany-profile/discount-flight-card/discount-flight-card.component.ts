import { Component, OnInit, Input } from '@angular/core';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AvioService } from 'src/app/_services/avio.service';
import { ReservationDialogComponent } from 'src/app/_dialogs/_avio_company_dialogs/reservation-dialog/reservation-dialog.component';

@Component({
  selector: 'app-discount-flight-card',
  templateUrl: './discount-flight-card.component.html',
  styleUrls: ['./discount-flight-card.component.css']
})
export class DiscountFlightCardComponent implements OnInit {
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
    
      this.dialog.open(ReservationDialogComponent, {
        width: '700px',
        height: '600px',
        data: {flight: this.flight, company: this.company, discount: true}
      });
  
    }
 
  }


