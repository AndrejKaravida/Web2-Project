import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CarrentalService } from 'src/app/_services/carrental.service';

@Component({
  selector: 'app-remove-destinations-dialog',
  templateUrl: './remove-destinations-dialog.component.html',
  styleUrls: ['./remove-destinations-dialog.component.css']
})
export class RemoveDestinationsDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<RemoveDestinationsDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private alertify: AlertifyService,
              private rentalService: CarrentalService) { }

  ngOnInit() {
  }

}
