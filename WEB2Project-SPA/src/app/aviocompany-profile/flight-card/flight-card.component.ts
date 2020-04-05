import { Component, OnInit, Input } from '@angular/core';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/flight';
import { EditFlightDialogComponent } from 'src/app/_dialogs/editrentalcompanydialog/edit-flight-dialog/edit-flight-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-flight-card',
  templateUrl: './flight-card.component.html',
  styleUrls: ['./flight-card.component.css']
})
export class FlightCardComponent implements OnInit {
  faPlaneDeparture = faPlaneDeparture;
  faPlaneArrival = faPlaneArrival;
  @Input() flight: Flight;
 
  constructor(private dialog: MatDialog) { }

  ngOnInit() {
  }

  


}
