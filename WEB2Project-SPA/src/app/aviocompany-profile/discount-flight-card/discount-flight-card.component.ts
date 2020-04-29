import { Component, OnInit, Input } from '@angular/core';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { Flight } from 'src/app/_models/flight';
import { AvioCompany } from 'src/app/_models/aviocompany';
import { ActivatedRoute } from '@angular/router';

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

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
   
  }

}
