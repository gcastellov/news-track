import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';

import { RelationshipExecutorComponent } from './relationship-executor.component';
import { httpLoaderFactory } from '../../app.module';
import { BackendApiService } from '../../services/backend-api.service';
import { TestBedHelper } from '../../testing/testbed.helper';
import { Observable } from 'rxjs';

describe('RelationshipExecutorComponent', () => {
  let component: RelationshipExecutorComponent;
  let fixture: ComponentFixture<RelationshipExecutorComponent>;

  const apiServiceMock = <BackendApiService> {
    processSuggestions: () => new Observable(observer => observer.complete)
  };

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RelationshipExecutorComponent ],
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
        { provide: BackendApiService, useFactory: () => apiServiceMock }
       ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationshipExecutorComponent);
    component = fixture.componentInstance;
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show proper message when it executes suggestions process', () => {
    const processSuggestionsMock = spyOn(apiServiceMock, 'processSuggestions').and
      .callFake(() => new Observable<boolean>(observer => observer.next(true)));

      component.execute();

      expect(processSuggestionsMock).toHaveBeenCalled();
      expect(component.isExecuted).toBeTruthy();
  });
});
