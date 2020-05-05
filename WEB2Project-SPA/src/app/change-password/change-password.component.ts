import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomValidators } from '../custom-validators';
import { UpdatePassword } from '../_models/_userModels/updatePassword';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { Router } from '@angular/router';
import * as ChangePassword from '../_shared/changePassword.actions';
import { Store } from '@ngrx/store';
import * as fromRoot from '../app.reducer';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  formGroup: FormGroup;
  updatePassword: UpdatePassword = {
    email: '',
    password: ''
  };

  constructor(private formBuilder: FormBuilder, private authService: AuthService,
              private userService: UserService, private router: Router,
              private store: Store<fromRoot.State>) { }

  ngOnInit() {
    this.formGroup = this.formBuilder.group({
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

  submitForm() {
    this.authService.userProfile$.subscribe(data => {
      this.updatePassword.email = data.email;
    });
    this.updatePassword.password = this.formGroup.get('password').value;

    this.userService.updatePassword(this.updatePassword).subscribe(res => {
      this.router.navigate(['home']);
      this.store.dispatch(new ChangePassword.SetNotNeedToChangePassword());
    });

  }

}
