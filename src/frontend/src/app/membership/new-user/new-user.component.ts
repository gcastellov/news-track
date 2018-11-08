import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { BackendApiService } from '../../services/backend-api.service';
import { CreateIdentityRequestDto } from '../../services/Dtos/CreateIdentityRequestDto';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.less']
})
export class NewUserComponent {

  usrForm: FormGroup;
  failureReason: number;

  constructor(private _apiService: BackendApiService, fBuilder: FormBuilder) {
    this.failureReason = 0;
    this.usrForm = fBuilder.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password1: ['', [Validators.required, Validators.minLength(6)]],
      password2: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.usrForm.setValidators(form => {
      const pwd1 = form.get('password1').value;
      const pwd2 = form.get('password2').value;
      if (pwd1 === pwd2) {
        return null;
      }

      return { 'status': false };
    });
  }

  createUser() {
    const request = new CreateIdentityRequestDto();
    request.username = this.usrForm.get('username').value;
    request.email = this.usrForm.get('email').value;
    request.password = this.usrForm.get('password1').value;
    request.confirmPassword = this.usrForm.get('password2').value;
    this._apiService.createUser(request).subscribe(p => {
        if (p.isSuccessful) {
          this.failureReason = -1;
          this.usrForm.controls['username'].setValue('');
          this.usrForm.controls['email'].setValue('');
          this.usrForm.controls['password1'].setValue('');
          this.usrForm.controls['password2'].setValue('');
        } else {
          this.failureReason = p.failure;
        }
    });
  }

}
