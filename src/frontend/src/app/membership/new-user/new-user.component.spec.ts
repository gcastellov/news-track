import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { NewUserComponent } from './new-user.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { TestBedHelper } from '../../testing/testbed.helper';
import { Envelope, UntypedEnvelope } from '../../services/Dtos/Envelope';
import { Observable } from 'rxjs';

describe('NewUserComponent', () => {
  let component: NewUserComponent;
  let fixture: ComponentFixture<NewUserComponent>;

  const apiServiceMock = <BackendApiService>{
    createUser: (r) => new Observable<UntypedEnvelope>(observer => observer.complete)
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ NewUserComponent ],
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
    const email = 'MyEmail';
    component.usrForm.controls['username'].setValue(username);
    component.usrForm.controls['email'].setValue(email);

    const responseDto = new UntypedEnvelope();

    const createUserMock = spyOn(apiServiceMock, 'createUser').and
      .callFake(() => new Observable<UntypedEnvelope>(o => o.next(new Envelope(responseDto))));

    component.createUser();

    expect(createUserMock).toHaveBeenCalled();
    expect(component.usrForm.get('username')?.value).toBe('');
    expect(component.usrForm.get('email')?.value).toBe('');
  });

  it('should show proper message when creating user fails', () => {
    const username = 'MyUsername';
    const email = 'MyEmail';
    const errorCode = 1;
    
    component.usrForm.controls['username'].setValue(username);
    component.usrForm.controls['email'].setValue(email);

    const changePasswordMock = spyOn(apiServiceMock, 'createUser').and
      .callFake(() => new Observable<UntypedEnvelope>(o => o.next(Envelope.AsFailure(errorCode))));

    component.createUser();

    expect(changePasswordMock).toHaveBeenCalled();
    expect(component.failureReason).toBe(errorCode);
    expect(component.usrForm.get('username')?.value).toBe(username);
    expect(component.usrForm.get('email')?.value).toBe(email);
  });
});
