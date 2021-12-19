import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { httpLoaderFactory } from 'src/app/app.module';
import { AuthenticationApiService } from 'src/app/services/authentication-api.service';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { Envelope } from 'src/app/services/Dtos/Envelope';
import { DataBuilder } from 'src/app/testing/data.builder';
import { CommentThreadComponent } from './comment-thread.component';

describe('CommentThreadComponent', () => {
  let component: CommentThreadComponent;
  let fixture: ComponentFixture<CommentThreadComponent>;

  const draft = DataBuilder.getDraftsDto()[0];
  const comment = DataBuilder.getComment(draft.id);
  const replies = DataBuilder.getComments(draft.id);

  const authServiceMock = <AuthenticationApiService> {
    isAuthenticated: () => true
  };

  const apiServiceMock = <BackendApiService> {
    getComment: (id) => new Observable(observer => observer.next(new Envelope(comment))),
    getCommentsByCommentId: (id, take, skip) => new Observable(observer => observer.next(new Envelope(replies)))
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentThreadComponent ],
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
        { provide: AuthenticationApiService, useFactory: () => authServiceMock },
        { provide: ActivatedRoute, useValue: {
          params: {
            subscribe: (fn: (value: Params) => void) => fn({
                id: comment.id,
            }),
          }
        }}
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentThreadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
