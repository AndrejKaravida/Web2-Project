import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Destination } from 'src/app/_models/_avioModels/destination';
import { AvioService } from 'src/app/_services/avio.service';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-destinations-dialog',
  templateUrl: './destinations-dialog.component.html',
  styleUrls: ['./destinations-dialog.component.css']
})
export class DestinationsDialogComponent implements OnInit {
  displayedColumns: string[] = ['city', 'country', 'remove'];
  dataSource: MatTableDataSource<Destination>;

  constructor(public dialogRef: MatDialogRef<DestinationsDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any,
              private alertify: AlertifyService, private avioService: AvioService) { }
 
  ngOnInit() {
    this.avioService.getAllDestinations().subscribe(res => {
      this.dataSource = new MatTableDataSource(res);
    });
  }

   Remove(rowid: number){
  //   if (rowid > -1) {
  //    this.destinations.splice(rowid, 1);
  //    this.dataSource = new MatTableDataSource(this.destinations);
   }

}
