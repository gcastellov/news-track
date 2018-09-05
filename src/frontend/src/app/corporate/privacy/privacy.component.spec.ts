import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { PrivacyComponent } from './privacy.component';
import { httpLoaderFactory } from '../../app.module';
import { AppSettingsService } from '../../services/app-settings.service';
import { Observable } from '../../../../node_modules/rxjs/Observable';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';

describe('PrivacyComponent', () => {
  let component: PrivacyComponent;
  let fixture: ComponentFixture<PrivacyComponent>;

  const settings = DataBuilder.getSettings();
  const settingsServiceMock = <AppSettingsService> {
    getSettings: () => new Observable(observer => observer.next(settings))
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrivacyComponent ],
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
        RouterTestingModule
      ],
      providers: [
        { provide: AppSettingsService, useFactory: () => settingsServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivacyComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
