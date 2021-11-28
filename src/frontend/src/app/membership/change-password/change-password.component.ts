import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BackendApiService } from '../../services/backend-api.service';
import { ChangePassworRequestDto } from '../../services/Dtos/ChangePasswordRequestDto';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.less']
})
export class ChangePasswordComponent {

  pwdForm: FormGroup;
  failureReason: number;

  constructor(private _apiService: BackendApiService, fBuilder: FormBuilder) {
    this.failureReason = 0;
    this.pwdForm = fBuilder.group({
      currentPassword: ['', Validators.required],
      password1: ['', [Validators.required, Validators.minLength(6)]],
      password2: ['', [Validators.required, Validators.minLength(6)]]
    });

    this.pwdForm.setValidators(form => {
      const pwd1 = form.get('password1')?.value;
      const pwd2 = form.get('password2')?.value;
      if (pwd1 === pwd2) {
        return null;
      }

      return { 'status': false };
    });
  }

  changePassword() {
    const request = new ChangePassworRequestDto();
    request.currentPassword = this.pwdForm.get('currentPassword')?.value;
    request.password = this.pwdForm.get('password1')?.value;
    request.confirmPassword = this.pwdForm.get('password2')?.value;
    this._apiService.changePassword(request).subscribe(p => {
        if (p.isSuccessful) {
          this.failureReason = -1;
          this.pwdForm.controls['currentPassword'].setValue('');
          this.pwdForm.controls['password1'].setValue('');
          this.pwdForm.controls['password2'].setValue('');
        } else if (p.error) {
          this.failureReason = p.error.code;
        }
    });
  }
}
