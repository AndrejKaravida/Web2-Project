import { Component, OnInit, Input } from '@angular/core';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/flight';
import { AvioCompany } from 'src/app/_models/aviocompany';

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
 
  constructor() { }

  ngOnInit() {
    console.log(this.flight);
  }

 

}
