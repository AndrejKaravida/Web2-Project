import { Component, OnInit } from '@angular/core';
import { Reservation } from '../_models/carreservation';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent implements OnInit {
  reservations: Reservation[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      const key = 'reservations';
      this.reservations = data[key];
    });

    console.log(this.reservations);
  }

}
