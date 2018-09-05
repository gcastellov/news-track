import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs/Observable';

import { BrowsingComponent } from './browsing.component';
import { httpLoaderFactory } from '../app.module';
import { BackendApiService } from '../services/backend-api.service';
import { BrowsingDraft } from './bowsing-draft';
import { BrowsingItemComponent } from './browsing-item/browsing-item.component';
import { BrowsingPictureComponent } from './browsing-picture/browsing-picture.component';
import { BrowsingTitleComponent } from './browsing-title/browsing-title.component';
import { BrowsingParagraphComponent } from './browsing-paragraph/browsing-paragraph.component';
import { BrowsingRelationshipComponent } from './browsing-relationship/browsing-relationship.component';
import { BrowsingRelationshipDraftComponent } from './browsing-relationship-draft/browsing-relationship-draft.component';
import { SharedModule } from '../shared/shared.module';
import { TestBedHelper } from '../testing/testbed.helper';
import { DataBuilder } from '../testing/data.builder';

describe('BrowsingComponent', () => {
  let component: BrowsingComponent;
  let fixture: ComponentFixture<BrowsingComponent>;

  const tags = DataBuilder.getTags();
  const apiServiceMock = <BackendApiService>{
    getTags: () => new Observable<string[]>(observer => observer.next(tags))
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
        { provide: BrowsingDraft, useClass: BrowsingDraft }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowsingComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
