import { async, ComponentFixture, TestBed } from '@angular/core/testing';
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
    getItem: (key) => 'exists'
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CookieConsentComponent ],
      imports: [
        HttpClientTestingModule,
        NgbModule.forRoot(),
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
});
