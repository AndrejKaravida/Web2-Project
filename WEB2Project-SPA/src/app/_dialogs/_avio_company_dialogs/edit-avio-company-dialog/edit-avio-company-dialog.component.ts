import { Component, OnInit, Inject, Input } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AvioCompany } from 'src/app/_models/_avioModels/aviocompany';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Flight } from 'src/app/_models/_avioModels/flight';
import { AvioService } from 'src/app/_services/avio.service';

@Component({
  selector: 'app-edit-avio-company-dialog',
  templateUrl: './edit-avio-company-dialog.component.html',
  styleUrls: ['./edit-avio-company-dialog.component.css']
})
export class EditAvioCompanyDialogComponent {

  company: AvioCompany;

  @Input() tip: Flight;
  constructor( public dialogRef: MatDialogRef<EditAvioCompanyDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: AvioCompany) { }

 
}