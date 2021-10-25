import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';

import { BrowsingRelationshipComponent } from './browsing-relationship.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { BrowsingDraft } from '../browsing-draft/bowsing-draft';
import { DraftListDto } from '../../services/Dtos/DraftListDto';
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
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';
import { Envelope } from '../../services/Dtos/Envelope';
import { Observable } from 'rxjs';

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
      return new Observable<Envelope<DraftListDto>>(observer => observer.next(new Envelope(draftList)));
    }
  };

  const selectedDraft = new DraftRelationshipDto('my-id', 'my-title', 'http://some.domain.com/resource');

  const settingsServiceMock = <AppSettingsService>{
    getExpressions: () => new Observable<string[]>(observer => observer.next(['expression one', 'expression two']))
  };

  beforeEach(waitForAsync(() => {
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
        { provide: BrowsingDraft, useClass: BrowsingDraft }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsingRelationshipComponent);
    component = fixture.componentInstance;
    component.draft = new BrowsingDraft(apiServiceMock);
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add a related draft when is selected', () => {
    component.onSelectionChange(selectedDraft);
    expect(component.draft.relationship.indexOf(selectedDraft)).toBeGreaterThan(-1);
  });

  it('should remove a related draft when is unselected', () => {
    component.draft.relationship.push(selectedDraft);
    component.onSelectionChange(selectedDraft);
    expect(component.draft.relationship.indexOf(selectedDraft)).toBe(-1);
  });
});
