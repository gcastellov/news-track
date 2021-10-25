import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SearchComponent } from './search.component';
import { BackendApiService } from '../../services/backend-api.service';
import { httpLoaderFactory } from '../../app.module';
import { DraftComponent } from '../draft/draft.component';
import { TagsComponent } from '../tags/tags.component';
import { WebsitesComponent } from '../websites/websites.component';
import { PaginatorComponent } from '../../shared/paginator/paginator.component';
import { DraftFooterComponent } from '../draft-footer/draft-footer.component';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';
import { LoaderComponent } from '../../shared/loader/loader.component';
import { Envelope } from '../../services/Dtos/Envelope';
import { FailureComponent } from '../../shared/failure/failure.component';
import { Observable } from 'rxjs';

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;

  const expressions = DataBuilder.getExpressions();
  const draftList = DataBuilder.getDraftListDto();
  const tagStats = DataBuilder.getTagsStatsDto();
  const websiteStats = DataBuilder.getWebsitesStatsDto();

  const apiServiceMock = <BackendApiService>{
    advancedSearch: (website, pattern, tags, page, take) => new Observable(observer => observer.next(new Envelope(draftList))),
    getStatsTags: () => new Observable(observer => observer.next(new Envelope(tagStats))),
    getWebsites: (take) => new Observable(observer => observer.next(new Envelope(websiteStats)))
  };

  const storageServiceMock = <StorageService>{
    getItem: (key) => 'exists'
  };
  const settingsServiceMock = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(expressions))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [
        SearchComponent,
        DraftComponent,
        DraftFooterComponent,
        TagsComponent,
        WebsitesComponent,
        PaginatorComponent,
        LoaderComponent,
        FailureComponent
       ],
      imports: [
        FormsModule,
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
        { provide: StorageService, useFactory: () => storageServiceMock },
        { provide: AppSettingsService, useFactory: () => settingsServiceMock },
        { provide: ActivatedRoute, useValue: {
          queryParams: {
            subscribe: (fn: (value: Params) => void) => fn({
              tags: '',
              website: ''
            })
          }
        }}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should search with proper criteria on page changes', () => {
    const newPage = 3;
    component.website = 'http://some.domain.com';
    component.page = 1;
    component.take = 10;

    const searchMock = spyOn(apiServiceMock, 'advancedSearch').and.callThrough();
    component.onPageChange(newPage);
    expect(searchMock).toHaveBeenCalledWith(component.website, component.pattern, component.tags, component.page, component.take);
    expect(component.page).toBe(newPage);
  });
});
