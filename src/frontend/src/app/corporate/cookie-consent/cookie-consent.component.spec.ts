import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { httpLoaderFactory } from '../../app.module';
import { CookieConsentComponent } from './cookie-consent.component';
import { StorageService } from '../../services/storage.service';
import { TestBedHelper } from '../../testing/testbed.helper';

describe('CookieConsentComponent', () => {
  let component: CookieConsentComponent;
  let fixture: ComponentFixture<CookieConsentComponent>;

  const storageServiceMock = <StorageService>{
    getItem: (key) => '',
    setItem: (key, value) => {}
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CookieConsentComponent ],
      imports: [
        HttpClientTestingModule,
        NgbModule,
        TranslateModule.forRoot({
          loader: {
              provide: TranslateLoader,
              useFactory: httpLoaderFactory,
              deps: [HttpClient]
          }
        }),
        RouterTestingModule.withRoutes([])
      ],
      providers: [
        { provide: StorageService, useFactory: () => storageServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CookieConsentComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should read storage to check for preferences', () => {
     expect(component.isAccepted).toBeFalsy();
  });

  it('should save preferences to the storage when giving consent', () => {
    const storageSaveMock = spyOn(storageServiceMock, 'setItem').and.callThrough();
    component.accept();
    expect(storageSaveMock).toHaveBeenCalled();
    expect(component.isAccepted).toBeTruthy();
  });
});
