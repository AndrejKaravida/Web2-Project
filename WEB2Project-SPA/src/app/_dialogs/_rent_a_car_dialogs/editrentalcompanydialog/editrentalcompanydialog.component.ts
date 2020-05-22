import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarCompany } from 'src/app/_models/_carModels/carcompany';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/custom-validators';

@Component({
  selector: 'app-editrentalcompanydialog',
  templateUrl: './editrentalcompanydialog.component.html',
  styleUrls: ['./editrentalcompanydialog.component.css']
})
export class EditrentalcompanydialogComponent implements OnInit {
  formGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<EditrentalcompanydialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CarCompany, private formBuilder: FormBuilder) {}


  ngOnInit(): void {
    this.formGroup = this.formBuilder.group({
      name: [this.data.name, [Validators.minLength(3), Validators.maxLength(20), Validators.required]],
      promoDescription: [this.data.promoDescription, [Validators.minLength(5), Validators.maxLength(30), Validators.required]],
      monthlyDiscount: [this.data.monthRentalDiscount.toString(),
         [Validators.min(0), Validators.max(99), Validators.required, CustomValidators.numberValidator]],
      weeklyDiscount: [this.data.weekRentalDiscount.toString(),
         [Validators.min(0), Validators.max(99), Validators.required, CustomValidators.numberValidator]]
      });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  editCompany() {
    this.data.name = this.formGroup.get('name').value;
    this.data.promoDescription = this.formGroup.get('promoDescription').value;
    this.data.monthRentalDiscount = this.formGroup.get('monthlyDiscount').value;
    this.data.weekRentalDiscount = this.formGroup.get('weeklyDiscount').value;

    this.dialogRef.close(this.data);
  }

}
