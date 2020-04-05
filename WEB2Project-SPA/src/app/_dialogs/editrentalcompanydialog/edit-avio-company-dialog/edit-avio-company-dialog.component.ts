import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AvioCompany } from 'src/app/_models/aviocompany';

@Component({
  selector: 'app-edit-avio-company-dialog',
  templateUrl: './edit-avio-company-dialog.component.html',
  styleUrls: ['./edit-avio-company-dialog.component.css']
})
export class EditAvioCompanyDialogComponent implements OnInit {

  constructor( public dialogRef: MatDialogRef<EditAvioCompanyDialogComponent>,
               @Inject(MAT_DIALOG_DATA) public data: AvioCompany) { }

  ngOnInit() {
  }

}