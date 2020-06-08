import { Component, Inject, OnInit } from '@angular/core';
import { Branch } from 'src/app/_models/_shared/branch';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarrentalService } from 'src/app/_services/carrental.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-new-branch-dialog',
  templateUrl: './add-new-branch-dialog.component.html',
  styleUrls: ['./add-new-branch-dialog.component.css']
})
export class AddNewBranchDialogComponent implements OnInit {
 formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AddNewBranchDialogComponent>, private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any, private rentalService: CarrentalService,
    private alertify: AlertifyService) {}

    ngOnInit() {
      this.formGroup = this.formBuilder.group({
        city: ['', [Validators.minLength(2), Validators.maxLength(30), Validators.required]],
        country: ['', [Validators.minLength(2), Validators.maxLength(30), Validators.required]],
        address: ['', [Validators.minLength(5), Validators.maxLength(50), Validators.required]],
        mapString: ['',  Validators.maxLength(50)],
      });
    }

    onClose(): void {
      this.dialogRef.close();
   }

   onAdd() {
    const newBranch: Branch = {
      city: this.formGroup.get('city').value,
      country: this.formGroup.get('country').value,
      address: this.formGroup.get('address').value,
      mapString: this.formGroup.get('mapString').value,
      id: 0
    };

    this.rentalService.addNewBranch(this.data.id, newBranch).subscribe(res => { 
      this.alertify.success('Successfully added new branch!');
      this.dialogRef.close();
    }, error => {
      let errorMessage = '';

      for (let err of error.error.errors) {
       errorMessage += err.message;
       errorMessage += '\n';
      }
      this.alertify.error(errorMessage);
      this.dialogRef.close();
    });
  }

}
