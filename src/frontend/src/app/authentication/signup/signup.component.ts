import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { CreateIdentityRequestDto } from 'src/app/services/Dtos/CreateIdentityRequestDto';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.less']
})
export class SignupComponent {

  usrForm: FormGroup;
  failureReason: number;
  isCompleted: boolean;

  constructor(private _apiService: BackendApiService, fBuilder: FormBuilder) {
    this.failureReason = 0;
    this.isCompleted = false;
    this.usrForm = fBuilder.group({
      username: ['', Validators.compose([Validators.required, Validators.minLength(3)])],
      email: ['', Validators.compose([Validators.required, Validators.email])]
    });
  }

  createUser() {
    const request = new CreateIdentityRequestDto();
    request.username = this.usrForm.get('username')?.value;
    request.email = this.usrForm.get('email')?.value;
    this._apiService.signUp(request).subscribe(p => {
        this.isCompleted = p.isSuccessful;
        if (!this.isCompleted && p.error) {
          this.failureReason = p.error.code;
        }
    });
  }
}
