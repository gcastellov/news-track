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
      email: ['', Validators.required]
    });
  }

  createUser() {
    const request = new CreateIdentityRequestDto();
    request.username = this.usrForm.get('username')?.value;
    request.email = this.usrForm.get('email')?.value;
    this._apiService.createUser(request).subscribe(p => {
        if (p.isSuccessful) {
          this.failureReason = -1;
          this.usrForm.controls['username'].setValue('');
          this.usrForm.controls['email'].setValue('');
        } else if (p.error) {
          this.failureReason = p.error.code;
        }
    });
  }

}
