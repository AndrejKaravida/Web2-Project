import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from 'src/app/custom-validators';

@Component({
  selector: 'app-add-new-company-dialog',
  templateUrl: './add-new-company-dialog.component.html',
  styleUrls: ['./add-new-company-dialog.component.css']
})
export class AddNewCompanyDialogComponent implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AddNewCompanyDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.firstFormGroup = this.formBuilder.group({
      name: ['', [Validators.minLength(2), Validators.maxLength(30), Validators.required]],
      address: ['', [Validators.minLength(5), Validators.maxLength(30), Validators.required]],
      city: ['', [Validators.minLength(2), Validators.maxLength(30), Validators.required]],
      country: ['', [Validators.minLength(2), Validators.maxLength(30), Validators.required]],
      mapString: ['', [Validators.maxLength(30)]]
    });

    this.secondFormGroup = this.formBuilder.group({
      firstName: ['', [Validators.minLength(2), Validators.maxLength(15), Validators.required]],
      lastName: ['', [Validators.minLength(2), Validators.maxLength(15), Validators.required]],
      email: ['', [Validators.email, Validators.required]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        CustomValidators.patternValidator(/\d/, { hasNumber: true }),
        CustomValidators.patternValidator(/[A-Z]/, { hasCapitalCase: true }),
        CustomValidators.patternValidator(/[a-z]/, { hasSmallCase: true }),
       ]],
       confirmPassword: ['', Validators.required]
      }, {validator: CustomValidators.passwordMatchValidator});
  }

    onNoClick(): void {
    this.dialogRef.close();
  }

  addCompany() {
    const newCompany = {
      name: this.firstFormGroup.get('name').value,
      address: this.firstFormGroup.get('address').value,
      city: this.firstFormGroup.get('city').value,
      country: this.firstFormGroup.get('country').value,
      mapString: this.firstFormGroup.get('mapString').value
    };

    const companyAdmin = {
      firstName: this.secondFormGroup.get('firstName').value,
      lastName: this.secondFormGroup.get('lastName').value,
      email: this.secondFormGroup.get('email').value,
      password: this.secondFormGroup.get('password').value
    };

    this.dialogRef.close({newCompany, companyAdmin, selectedFile: this.data.selectedFile});
  }

  onFileSelected(event) {
    this.data.selectedFile = event.target.files[0] as File;
  }

}
