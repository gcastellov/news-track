import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { DraftComponent } from './draft.component';
import { DraftFooterComponent } from '../draft-footer/draft-footer.component';
import { BackendApiService } from '../../services/backend-api.service';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';
import { httpLoaderFactory } from '../../app.module';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';
import { Observable } from 'rxjs';

describe('DraftComponent', () => {
  let component: DraftComponent;
  let fixture: ComponentFixture<DraftComponent>;

  const draft = DataBuilder.getDraftsDto()[0];
  const expressions = DataBuilder.getExpressions();
  const apiServiceMock = <BackendApiService>{};
  const storageService = <StorageService>{
    getItem: (key) => 'existing'
  };

  const settingsService = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(expressions))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [
        DraftComponent,
        DraftFooterComponent
      ],
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
        { provide: BackendApiService, useFactory: () => apiServiceMock },
        { provide: StorageService, useFactory: () => storageService },
        { provide: AppSettingsService, useFactory: () => settingsService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DraftComponent);
    component = fixture.componentInstance;
    component.draft = draft;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
