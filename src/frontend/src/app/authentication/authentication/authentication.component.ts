import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, Form } from '@angular/forms';
import { AuthenticationApiService } from '../../services/authentication-api.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationDto } from '../../services/Dtos/AuthenticationDto';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.less']
})
export class AuthenticationComponent implements OnInit {
  authForm: FormGroup;
  failureReason: number;
  hasConfirmed: boolean;

  constructor(
    private _authenticationService: AuthenticationApiService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute,
    _fBuilder: FormBuilder) {
    this.failureReason = 0;
    this.hasConfirmed = false;
    this.authForm = _fBuilder.group({
      username: ['', Validators.email],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this._activatedRoute.queryParams.subscribe(params => {
      if (params.confirmed) {
        this.hasConfirmed = true;
        this.authForm.setValue({
          username: params.email,
          password: ''
        });
      }
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
