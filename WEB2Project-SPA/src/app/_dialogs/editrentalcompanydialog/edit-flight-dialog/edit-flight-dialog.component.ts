import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Flight } from 'src/app/_models/flight';

@Component({
  selector: 'app-edit-flight-dialog',
  templateUrl: './edit-flight-dialog.component.html',
  styleUrls: ['./edit-flight-dialog.component.css']
})
export class EditFlightDialogComponent implements OnInit {

  constructor( public dialogRef: MatDialogRef<EditFlightDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: Flight) { }

  ngOnInit() {
  }

}
