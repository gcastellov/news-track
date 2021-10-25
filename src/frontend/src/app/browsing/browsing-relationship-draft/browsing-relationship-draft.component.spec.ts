import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { BrowsingRelationshipDraftComponent } from './browsing-relationship-draft.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';
import { Observable } from 'rxjs';

describe('BrowsingRelationshipDraftComponent', () => {
  let component: BrowsingRelationshipDraftComponent;
  let fixture: ComponentFixture<BrowsingRelationshipDraftComponent>;

  const expressions = DataBuilder.getExpressions();
  const draft = DataBuilder.getDraftsDto()[0];
  const apiServiceMock = <BackendApiService>{};
  const storageServiceMock = <StorageService>{};
  const settingsServiceMock = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(expressions))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ BrowsingRelationshipDraftComponent ],
      imports: [
        HttpClientTestingModule,
        FormsModule,
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
        { provide: StorageService, useFactory: () => storageServiceMock },
        { provide: AppSettingsService, useFactory: () => settingsServiceMock }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsingRelationshipDraftComponent);
    component = fixture.componentInstance;
    component.draft = draft;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
