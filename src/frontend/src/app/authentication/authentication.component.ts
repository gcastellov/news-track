import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, Form } from '@angular/forms';
import { AuthenticationApiService } from '../services/authentication-api.service';
import { Router } from '@angular/router';
import { AuthenticationDto } from '../services/Dtos/AuthenticationDto';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.less']
})
export class AuthenticationComponent {
  authForm: FormGroup;
  failureReason: number;

  constructor(
    private _authenticationService: AuthenticationApiService,
    private _router: Router,
    _fBuilder: FormBuilder) {
    this.failureReason = 0;
    this.authForm = _fBuilder.group({
      username: ['', Validators.email],
      password: ['', Validators.required]
    });
  }

  onSignIn() {
    this.failureReason = 0;
    if (this.authForm.valid) {
      const request = new AuthenticationDto();
      request.username = this.authForm.get('username')?.value;
      request.password = this.authForm.get('password')?.value;
      this._authenticationService.authenticate(request).subscribe(d => {
        if (d.isSuccess) {
          this._router.navigateByUrl('/membership/member');
        } else {
          this.failureReason = d.failureReason;
        }
      });
    }
  }
}
