import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { BrowsingComponent } from './browsing.component';
import { httpLoaderFactory } from '../app.module';
import { BackendApiService } from '../services/backend-api.service';
import { BrowsingDraft } from './browsing-draft/bowsing-draft';
import { BrowsingItemComponent } from './browsing-item/browsing-item.component';
import { BrowsingPictureComponent } from './browsing-picture/browsing-picture.component';
import { BrowsingTitleComponent } from './browsing-title/browsing-title.component';
import { BrowsingParagraphComponent } from './browsing-paragraph/browsing-paragraph.component';
import { BrowsingRelationshipComponent } from './browsing-relationship/browsing-relationship.component';
import { BrowsingRelationshipDraftComponent } from './browsing-relationship-draft/browsing-relationship-draft.component';
import { SharedModule } from '../shared/shared.module';
import { TestBedHelper } from '../testing/testbed.helper';
import { DataBuilder } from '../testing/data.builder';
import { IBrowseResult } from '../services/Dtos/IBrowseResult';
import { WebsiteDto } from '../services/Dtos/WebsiteDto';
import { Envelope } from '../services/Dtos/Envelope';
import { Observable } from 'rxjs';

describe('BrowsingComponent', () => {
  let component: BrowsingComponent;
  let fixture: ComponentFixture<BrowsingComponent>;

  const apiServiceMock = <BackendApiService>{
    getTags: () => new Observable<Envelope<string[]>>(observer => observer.next(new Envelope(DataBuilder.getTags()))),
    checkWebsite: (url) => new Observable<Envelope<WebsiteDto>>(observer => observer.complete),
    browse: (url) => new Observable<Envelope<IBrowseResult>>(observer => observer.complete)
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
        { provide: BrowsingDraft, useClass: BrowsingDraft }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsingComponent);
    component = fixture.componentInstance;
    component.draft.draftRequest.tags = DataBuilder.getTags();
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should add the desired tag if it is not already in the list', () => {
    const tag = 'my-tag';
    component.addTag(tag);
    expect(component.draft.draftRequest.tags.indexOf(tag)).toBeGreaterThan(-1);
  });

  it('should avoid adding the tag if it is a duplicate', () => {
    const tag = component.draft.draftRequest.tags[1];
    component.addTag(tag);
    expect(component.draft.draftRequest.tags.indexOf(tag)).toBeGreaterThan(-1);
    expect(component.draft.draftRequest.tags.length).toBe(7);
  });

  it('should remove a tag if it is present in the list', () => {
    const tag = component.draft.draftRequest.tags[1];
    component.removeTag(tag);
    expect(component.draft.draftRequest.tags.indexOf(tag)).toBe(-1);
    expect(component.draft.draftRequest.tags.length).toBe(6);
  });

  it('should avoid removing a tag that does not exist', () => {
    const tag = 'some-other-tag';
    component.removeTag(tag);
    expect(component.draft.draftRequest.tags.length).toBe(7);
  });

  it('should verify the requested URL and get the draft result', () => {
    component.url = 'http://www.some.domain.com/resource';
    const checkWebsiteMock = spyOn(apiServiceMock, 'checkWebsite').and.callThrough();
    const browseMock = spyOn(apiServiceMock, 'browse').and.returnValue(
      new Observable<Envelope<IBrowseResult>>(observer => observer.next(
        new Envelope({
        uri: component.url,
        titles: ['Title One', 'Title Two'],
        paragraphs: ['Paragraph One', 'Paragraph Two'],
        pictures: [ 'http://www.some.domain.com/pic.png']
      }))));

    component.onFind();

    expect(checkWebsiteMock).toHaveBeenCalledWith(component.url);
    expect(browseMock).toHaveBeenCalledWith(component.url);
  });

  it('should enable find button', () => {
    component.url = 'http://www.some.domain.com/resource';
    expect(component.isFindEnabled()).toBeTruthy();
  });

  it('should disable find button', () => {
    component.url = '';
    expect(component.isFindEnabled()).toBeFalsy();
  });

});
