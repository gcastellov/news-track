import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { WebsitesComponent } from './websites.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { WebsiteStatsDto } from '../../services/Dtos/WebsiteStatsDto';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';
import { Envelope } from '../../services/Dtos/Envelope';
import { Observable } from 'rxjs';

describe('WebsitesComponent', () => {
  let component: WebsitesComponent;
  let fixture: ComponentFixture<WebsitesComponent>;
  const websiteStats = DataBuilder.getWebsitesStatsDto();

  const apiServiceMock = <BackendApiService>{
    getWebsites: (take) => new Observable<Envelope<WebsiteStatsDto[]>>(observer => observer.next(new Envelope(websiteStats)))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ WebsitesComponent ],
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
        RouterTestingModule
      ],
      providers: [
        { provide: BackendApiService, useFactory: () => apiServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WebsitesComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
