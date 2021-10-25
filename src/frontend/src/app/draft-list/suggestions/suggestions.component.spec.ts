import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';


import { SuggestionsComponent } from './suggestions.component';
import { BackendApiService } from '../../services/backend-api.service';
import { httpLoaderFactory } from '../../app.module';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DraftComponent } from '../draft/draft.component';
import { DraftFooterComponent } from '../draft-footer/draft-footer.component';
import { DraftSuggestionIdsDto } from '../../services/Dtos/DraftSuggestionIdsDto';
import { DataBuilder } from '../../testing/data.builder';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { Envelope } from '../../services/Dtos/Envelope';
import { SharedModule } from '../../shared/shared.module';
import { Observable } from 'rxjs';

describe('SuggestionsComponent', () => {
  let component: SuggestionsComponent;
  let fixture: ComponentFixture<SuggestionsComponent>;
  const draftId = 'my-id';

  const suggestions = DataBuilder.getDraftSuggesionIdsDto(draftId);
  const apiServiceMock = <BackendApiService>{
    getAllDraftSuggestions: (id, take, skip) => new Observable<Envelope<DraftSuggestionIdsDto>>(
      observer => observer.next(new Envelope(suggestions))),
    getDraft: (id) => new Observable<Envelope<DraftDto>>(observer => observer.complete)
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SuggestionsComponent, DraftComponent, DraftFooterComponent ],
      imports: [
        FormsModule,
        SharedModule,
        InfiniteScrollModule,
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
        { provide: ActivatedRoute, useValue: {
          params: {
            subscribe: (fn: (value: Params) => void) => fn({
                id: draftId,
            }),
          }
        }}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SuggestionsComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
