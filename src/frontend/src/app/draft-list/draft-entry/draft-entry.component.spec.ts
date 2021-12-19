import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { DraftEntryComponent } from './draft-entry.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { DraftComponent } from '../draft/draft.component';
import { DraftFooterComponent } from '../draft-footer/draft-footer.component';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';
import { Envelope } from '../../services/Dtos/Envelope';
import { Observable } from 'rxjs';
import { AuthenticationApiService } from 'src/app/services/authentication-api.service';

describe('DraftEntryComponent', () => {
  let component: DraftEntryComponent;
  let fixture: ComponentFixture<DraftEntryComponent>;
  let getDraftMock: any;
  let setVisitMock: any;
  let getDraftRelationshipMock: any;
  let getDraftSuggestionsMock: any;

  const draft = DataBuilder.getDraftsDto()[0];
  const relatedDrafts = DataBuilder.getDraftDigestsDto().splice(0, 1);
  const suggestion = DataBuilder.getDraftSuggestionDto();
  const expressions = DataBuilder.getExpressions();
  const comments = DataBuilder.getComments(draft.id);
  
  const authServiceMock = <AuthenticationApiService> {
    isAuthenticated: () => true
  };

  const apiServiceMock = <BackendApiService> {
    getDraft: (id) => new Observable(observer => observer.next(new Envelope(draft))),
    setVisit: (id) => new Observable(observer => observer.complete),
    getDraftRelationship: (id) => new Observable(observer => observer.next(new Envelope(relatedDrafts))),
    getDraftSuggestions: (id, take) => new Observable(observer => observer.next(new Envelope(suggestion))),
    getCommentsByDraftId: (draftId, take, skip) => new Observable(observer => observer.next(new Envelope(comments)))
  };

  const storageService = <StorageService>{
    getItem: (key) => 'existing'
  };

  const settingsService = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(expressions))
  };

  beforeEach(waitForAsync(() => {
    getDraftMock = spyOn(apiServiceMock, 'getDraft').and.callThrough();
    setVisitMock = spyOn(apiServiceMock, 'setVisit').and.callThrough();
    getDraftRelationshipMock = spyOn(apiServiceMock, 'getDraftRelationship').and.callThrough();
    getDraftSuggestionsMock = spyOn(apiServiceMock, 'getDraftSuggestions').and.callThrough();

    TestBed.configureTestingModule({
      declarations: [
        DraftEntryComponent,
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
        { provide: AppSettingsService, useFactory: () => settingsService },
        { provide: AuthenticationApiService, useFactory: () => authServiceMock },
        { provide: ActivatedRoute, useValue: {
          params: {
            subscribe: (fn: (value: Params) => void) => fn({
                id: draft.id,
            }),
          }
        }}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DraftEntryComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize values for the requested draft', () => {
    expect(getDraftMock).toHaveBeenCalled();
    expect(setVisitMock).toHaveBeenCalled();
    expect(getDraftRelationshipMock).toHaveBeenCalled();
    expect(getDraftSuggestionsMock).toHaveBeenCalled();
  });
});
