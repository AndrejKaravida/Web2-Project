import { Component, OnInit, Inject, Input } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AvioCompany } from 'src/app/_models/aviocompany';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Flight } from 'src/app/_models/flight';

@Component({
  selector: 'app-edit-avio-company-dialog',
  templateUrl: './edit-avio-company-dialog.component.html',
  styleUrls: ['./edit-avio-company-dialog.component.css']
})
export class EditAvioCompanyDialogComponent implements OnInit {

  @Input() tip: Flight;
  constructor( public dialogRef: MatDialogRef<EditAvioCompanyDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: AvioCompany, 
               private alertify: AlertifyService) { }

  ngOnInit() {
    console.log(this.tip);
  }
  ChangeAlertify(){
    this.alertify.success('You have successfully changed avio data.');
  }
}