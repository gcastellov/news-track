import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { ChangePasswordComponent } from './change-password.component';
import { httpLoaderFactory } from '../../app.module';
import { TestBedHelper } from '../../testing/testbed.helper';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Observable';
import { ChangePasswordResponseDto } from '../../services/Dtos/ChangePasswordResponseDto';

describe('ChangePasswordComponent', () => {
  let component: ChangePasswordComponent;
  let fixture: ComponentFixture<ChangePasswordComponent>;

  const apiServiceMock = <BackendApiService>{
    changePassword: (r) => new Observable<ChangePasswordResponseDto>(observer => observer.complete)
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangePasswordComponent ],
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        NgbModule.forRoot(),
        TranslateModule.forRoot({
          loader: {
              provide: TranslateLoader,
              useFactory: httpLoaderFactory,
              deps: [HttpClient]
          }
        }),
        RouterTestingModule
      ],
      providers: [
        { provide: BackendApiService, useFactory: () => apiServiceMock }
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangePasswordComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should change password', () => {
    const currentPwd = 'MyCurrentPassword';
    const newPwd = 'MyNewPwd';
    component.pwdForm.controls['currentPassword'].setValue(currentPwd);
    component.pwdForm.controls['password1'].setValue(newPwd);
    component.pwdForm.controls['password2'].setValue(newPwd);

    const responseDto = new ChangePasswordResponseDto();
    responseDto.isSuccessful = true;

    const changePasswordMock = spyOn(apiServiceMock, 'changePassword').and
      .callFake(() => new Observable<ChangePasswordResponseDto>(o => o.next(responseDto)));

    component.changePassword();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.pwdForm.get('currentPassword').value).toBe('');
    expect(component.pwdForm.get('password1').value).toBe('');
    expect(component.pwdForm.get('password2').value).toBe('');
  });

  it('should show proper message when changing password fails', () => {
    const currentPwd = 'MyCurrentPassword';
    const newPwd = 'MyNewPwd';
    component.pwdForm.controls['currentPassword'].setValue(currentPwd);
    component.pwdForm.controls['password1'].setValue(newPwd);
    component.pwdForm.controls['password2'].setValue(newPwd);

    const responseDto = new ChangePasswordResponseDto();
    responseDto.isSuccessful = false;
    responseDto.failure = 1;

    const changePasswordMock = spyOn(apiServiceMock, 'changePassword').and
      .callFake(() => new Observable<ChangePasswordResponseDto>(o => o.next(responseDto)));

    component.changePassword();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.failureReason).toBe(responseDto.failure);
    expect(component.pwdForm.get('currentPassword').value).toBe(currentPwd);
    expect(component.pwdForm.get('password1').value).toBe(newPwd);
    expect(component.pwdForm.get('password2').value).toBe(newPwd);
  });
});
