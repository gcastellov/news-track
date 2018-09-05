import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/Observable';
import { SharedModule } from '../../shared/shared.module';

import { BrowsingRelationshipComponent } from './browsing-relationship.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { BrowsingDraft } from '../bowsing-draft';
import { DraftListDto } from '../../services/Dtos/DraftListDto';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { BrowsingComponent } from '../browsing.component';
import { BrowsingItemComponent } from '../browsing-item/browsing-item.component';
import { BrowsingPictureComponent } from '../browsing-picture/browsing-picture.component';
import { BrowsingTitleComponent } from '../browsing-title/browsing-title.component';
import { BrowsingParagraphComponent } from '../browsing-paragraph/browsing-paragraph.component';
import { BrowsingRelationshipDraftComponent } from '../browsing-relationship-draft/browsing-relationship-draft.component';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';
import { TestBedHelper } from '../../testing/testbed.helper';
import { DataBuilder } from '../../testing/data.builder';

describe('BrowsingRelationshipComponent', () => {
  let component: BrowsingRelationshipComponent;
  let fixture: ComponentFixture<BrowsingRelationshipComponent>;

  const drafts = DataBuilder.getDraftsDto();
  const storageServiceMock = <StorageService>{};
  const apiServiceMock = <BackendApiService> {
    getLatestDrafts: (page, take) => {
      const draftList = new DraftListDto();
      draftList.count = drafts.length;
      draftList.news = drafts;
      return new Observable<DraftListDto>(observer => observer.next(draftList));
    }
  };

  const settingsServiceMock = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(['expression one', 'expression two']))
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        BrowsingComponent,
        BrowsingItemComponent,
        BrowsingPictureComponent,
        BrowsingTitleComponent,
        BrowsingParagraphComponent,
        BrowsingRelationshipComponent,
        BrowsingRelationshipDraftComponent
      ],
      imports: [
        HttpClientTestingModule,
        FormsModule,
        ReactiveFormsModule,
        SharedModule,
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
        { provide: StorageService, useFactory: () => storageServiceMock },
        { provide: AppSettingsService, useFactory: () => settingsServiceMock },
        { provide: BrowsingDraft, useClass: BrowsingDraft }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsingRelationshipComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
