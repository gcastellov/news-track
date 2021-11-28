import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { AuthenticationComponent } from './authentication.component';
import { BackendApiService } from '../../services/backend-api.service';
import { AuthenticationApiService } from '../../services/authentication-api.service';
import { httpLoaderFactory } from '../../app.module';
import { TestBedHelper } from '../../testing/testbed.helper';
import { AuthenticationResult } from '../../services/Dtos/AuthenticationResult';
import { Observable } from 'rxjs';

describe('AuthenticationComponent', () => {
  let component: AuthenticationComponent;
  let fixture: ComponentFixture<AuthenticationComponent>;

  const authServiceMock = <AuthenticationApiService>{
    authenticate: (auth) => new Observable<AuthenticationResult>(observer => observer.complete)
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthenticationComponent ],
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule,
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
        BackendApiService,
        { provide: AuthenticationApiService, useFactory: () => authServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthenticationComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show proper failure message when authentication fails', () => {

    const username = 'my-user@domain.com';
    const authDto = new AuthenticationResult();
    authDto.username = username;
    authDto.failureReason = 1;

    const authSpy = spyOn(authServiceMock, 'authenticate').and.returnValue(
      new Observable<AuthenticationResult>(observer => observer.next(authDto))
    );

    component.authForm.setValue({username: username, password: 'my-password'});
    component.onSignIn();

    expect(authSpy).toHaveBeenCalledTimes(1);
  });
});
