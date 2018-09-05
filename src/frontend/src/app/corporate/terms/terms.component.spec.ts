import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/Observable';

import { httpLoaderFactory } from '../../app.module';
import { AppSettingsService } from '../../services/app-settings.service';
import { TermsComponent } from './terms.component';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';

describe('TermsComponent', () => {
  let component: TermsComponent;
  let fixture: ComponentFixture<TermsComponent>;

  const settings = DataBuilder.getSettings();
  const settingsServiceMock = <AppSettingsService> {
    getSettings: () => new Observable(observer => observer.next(settings))
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TermsComponent ],
      imports: [
        HttpClientTestingModule,
        NgbModule.forRoot(),
        TranslateModule.forRoot({
          loader: {
              provide: TranslateLoader,
              useFactory: httpLoaderFactory,
              deps: [HttpClient]
          }
        })
      ],
      providers: [
        { provide: AppSettingsService, useFactory: () => settingsServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TermsComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {

    expect(component).toBeTruthy();
  });
});
