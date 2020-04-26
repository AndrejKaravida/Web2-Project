import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-show-map-dialog',
  templateUrl: './show-map-dialog.component.html',
  styleUrls: ['./show-map-dialog.component.css']
})
export class ShowMapDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<ShowMapDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private sanitizer: DomSanitizer) {}

  onNoClick(): void {
    this.dialogRef.close();
  }

  photoURL() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.data.mapString);
  }
}
