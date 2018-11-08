import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { NewUserComponent } from './new-user.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Observable';
import { CreateIdentityResponseDto } from '../../services/Dtos/CreateIdentityResponseDto';
import { TestBedHelper } from '../../testing/testbed.helper';

describe('NewUserComponent', () => {
  let component: NewUserComponent;
  let fixture: ComponentFixture<NewUserComponent>;

  const apiServiceMock = <BackendApiService>{
    createUser: (r) => new Observable<CreateIdentityResponseDto>(observer => observer.complete)
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewUserComponent ],
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
    fixture = TestBed.createComponent(NewUserComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should create a user', () => {
    const username = 'MyUsername';
    const newPwd = 'MyNewPwd';
    const email = 'MyEmail';
    component.usrForm.controls['username'].setValue(username);
    component.usrForm.controls['email'].setValue(email);
    component.usrForm.controls['password1'].setValue(newPwd);
    component.usrForm.controls['password2'].setValue(newPwd);

    const responseDto = new CreateIdentityResponseDto();
    responseDto.isSuccessful = true;

    const createUserMock = spyOn(apiServiceMock, 'createUser').and
      .callFake(() => new Observable<CreateIdentityResponseDto>(o => o.next(responseDto)));

    component.createUser();

    expect(createUserMock).toHaveBeenCalled();
    expect(component.usrForm.get('username').value).toBe('');
    expect(component.usrForm.get('email').value).toBe('');
    expect(component.usrForm.get('password1').value).toBe('');
    expect(component.usrForm.get('password2').value).toBe('');
  });

  it('should show proper message when creating user fails', () => {
    const username = 'MyUsername';
    const newPwd = 'MyNewPwd';
    const email = 'MyEmail';
    component.usrForm.controls['username'].setValue(username);
    component.usrForm.controls['email'].setValue(email);
    component.usrForm.controls['password1'].setValue(newPwd);
    component.usrForm.controls['password2'].setValue(newPwd);

    const responseDto = new CreateIdentityResponseDto();
    responseDto.isSuccessful = false;
    responseDto.failure = 1;

    const changePasswordMock = spyOn(apiServiceMock, 'createUser').and
      .callFake(() => new Observable<CreateIdentityResponseDto>(o => o.next(responseDto)));

    component.createUser();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.failureReason).toBe(responseDto.failure);
    expect(component.usrForm.get('username').value).toBe(username);
    expect(component.usrForm.get('email').value).toBe(email);
    expect(component.usrForm.get('password1').value).toBe(newPwd);
    expect(component.usrForm.get('password2').value).toBe(newPwd);
  });
});
