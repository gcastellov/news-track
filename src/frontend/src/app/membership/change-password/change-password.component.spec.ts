import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
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
import { Envelope, UntypedEnvelope } from '../../services/Dtos/Envelope';
import { Observable } from 'rxjs';

describe('ChangePasswordComponent', () => {
  let component: ChangePasswordComponent;
  let fixture: ComponentFixture<ChangePasswordComponent>;

  const apiServiceMock = <BackendApiService>{
    changePassword: (r) => new Observable<UntypedEnvelope>(observer => observer.complete)
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ChangePasswordComponent ],
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        NgbModule,
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

    const changePasswordMock = spyOn(apiServiceMock, 'changePassword').and
      .callFake(() => new Observable<UntypedEnvelope>(o => o.next(new UntypedEnvelope())));

    component.changePassword();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.pwdForm.get('currentPassword')?.value).toBe('');
    expect(component.pwdForm.get('password1')?.value).toBe('');
    expect(component.pwdForm.get('password2')?.value).toBe('');
  });

  it('should show proper message when changing password fails', () => {
    const currentPwd = 'MyCurrentPassword';
    const newPwd = 'MyNewPwd';
    const errorCode = 1;
    component.pwdForm.controls['currentPassword'].setValue(currentPwd);
    component.pwdForm.controls['password1'].setValue(newPwd);
    component.pwdForm.controls['password2'].setValue(newPwd);

    const changePasswordMock = spyOn(apiServiceMock, 'changePassword').and
      .callFake(() => new Observable<UntypedEnvelope>(o => o.next(UntypedEnvelope.AsFailure(errorCode))));

    component.changePassword();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.failureReason).toBe(errorCode);
    expect(component.pwdForm.get('currentPassword')?.value).toBe(currentPwd);
    expect(component.pwdForm.get('password1')?.value).toBe(newPwd);
    expect(component.pwdForm.get('password2')?.value).toBe(newPwd);
  });
});
