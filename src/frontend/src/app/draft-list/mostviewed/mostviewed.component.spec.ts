import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/Observable';

import { MostviewedComponent } from './mostviewed.component';
import { BackendApiService } from '../../services/backend-api.service';
import { httpLoaderFactory } from '../../app.module';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';

describe('MostviewedComponent', () => {
  let component: MostviewedComponent;
  let fixture: ComponentFixture<MostviewedComponent>;

  const draftDigests = DataBuilder.getDraftDigestsDto();
  const apiServiceMock = <BackendApiService>{
    getMostViewed: (take) => new Observable(observer => observer.next(draftDigests))
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MostviewedComponent ],
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
        { provide: BackendApiService, useFactory: () => apiServiceMock },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MostviewedComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
