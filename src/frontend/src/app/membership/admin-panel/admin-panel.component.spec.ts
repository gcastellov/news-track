import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { httpLoaderFactory } from '../../app.module';

import { AdminPanelComponent } from './admin-panel.component';
import { RelationshipExecutorComponent } from '../relationship-executor/relationship-executor.component';
import { NewUserComponent } from '../new-user/new-user.component';
import { BackendApiService } from '../../services/backend-api.service';

describe('AdminPanelComponent', () => {
  let component: AdminPanelComponent;
  let fixture: ComponentFixture<AdminPanelComponent>;

  const apiServiceMock = <BackendApiService>{};

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [
        AdminPanelComponent,
        RelationshipExecutorComponent,
        NewUserComponent
      ],
      imports: [
        ReactiveFormsModule,
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
    fixture = TestBed.createComponent(AdminPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
