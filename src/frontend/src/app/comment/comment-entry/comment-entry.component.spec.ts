import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { httpLoaderFactory } from 'src/app/app.module';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { StorageService } from 'src/app/services/storage.service';
import { DataBuilder } from 'src/app/testing/data.builder';

import { CommentEntryComponent } from './comment-entry.component';

describe('CommentEntryComponent', () => {
  let component: CommentEntryComponent;
  let fixture: ComponentFixture<CommentEntryComponent>;

  const draft = DataBuilder.getDraftsDto()[0];
  const comment = DataBuilder.getComment(draft.id);

  const apiServiceMock = <BackendApiService> {
  };

  const storageService = <StorageService>{
    getItem: (key) => 'existing'
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentEntryComponent ],
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
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
