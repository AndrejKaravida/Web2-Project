import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { faPlaneDeparture } from '@fortawesome/free-solid-svg-icons';
import { faPlaneArrival } from '@fortawesome/free-solid-svg-icons';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-search-flight-dialog',
  templateUrl: './search-flight-dialog.component.html',
  styleUrls: ['./search-flight-dialog.component.css']
})
export class SearchFlightDialogComponent implements OnInit {
  faPlaneDeparture = faPlaneDeparture;
  faPlaneArrival = faPlaneArrival;
  faArrowRight = faArrowRight;

  constructor(public dialogRef: MatDialogRef<SearchFlightDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any ) { }


  ngOnInit() {

  }
  

  
ReserveFlight() {}
}
