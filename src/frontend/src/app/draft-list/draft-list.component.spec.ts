import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../shared/shared.module';

import { DraftListComponent } from './draft-list.component';
import { TagsComponent } from './tags/tags.component';
import { LatestComponent } from './latest/latest.component';
import { DraftComponent } from './draft/draft.component';
import { DraftEntryComponent } from './draft-entry/draft-entry.component';
import { SearchComponent } from './search/search.component';
import { WebsitesComponent } from './websites/websites.component';
import { MostviewedComponent } from './mostviewed/mostviewed.component';
import { DraftFooterComponent } from './draft-footer/draft-footer.component';
import { MostfuckingComponent } from './mostfucking/mostfucking.component';
import { BackendApiService } from '../services/backend-api.service';
import { httpLoaderFactory } from '../app.module';
import { DraftDigestDto } from '../services/Dtos/DraftDigestDto';
import { TagsStatsResponseDto } from '../services/Dtos/TagsStatsResponseDto';
import { WebsiteStatsDto } from '../services/Dtos/WebsiteStatsDto';
import { TestBedHelper } from '../testing/testbed.helper';
import { DataBuilder } from '../testing/data.builder';
import { Envelope } from '../services/Dtos/Envelope';
import { Observable } from 'rxjs';

describe('DraftListComponent', () => {
  let component: DraftListComponent;
  let fixture: ComponentFixture<DraftListComponent>;

  const drafts = DataBuilder.getDraftDigestsDto();
  const websites = DataBuilder.getWebsitesStatsDto();
  const tags = DataBuilder.getTagsStatsDto();

  const apiServiceMock = <BackendApiService> {
    getLatest: (take) => new Observable<Envelope<DraftDigestDto[]>>(observer => observer.next(new Envelope(drafts))),
    getStatsTags: () => new Observable<Envelope<TagsStatsResponseDto>>(observer => observer.next(new Envelope(tags))),
    getMostViewed: (take) => new Observable<Envelope<DraftDigestDto[]>>(observer => observer.next(new Envelope(drafts))),
    getMostFucking: (take) => new Observable<Envelope<DraftDigestDto[]>>(observer => observer.next(new Envelope(drafts))),
    getWebsites: (take) => new Observable<Envelope<WebsiteStatsDto[]>>(observer => observer.next(new Envelope(websites)))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [
        DraftListComponent,
        TagsComponent,
        LatestComponent,
        DraftComponent,
        DraftEntryComponent,
        SearchComponent,
        WebsitesComponent,
        MostviewedComponent,
        DraftFooterComponent,
        MostfuckingComponent
      ],
      imports: [
        FormsModule,
        SharedModule,
        NgbModule,
        HttpClientTestingModule,
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
    fixture = TestBed.createComponent(DraftListComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
