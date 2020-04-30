import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../custom-validators';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  frmSignup: FormGroup;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.frmSignup = this.formBuilder.group({
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

}
