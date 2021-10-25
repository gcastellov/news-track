import { TestBed, ComponentFixture, waitForAsync } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterTestingModule } from '@angular/router/testing';
import { CommonModule } from '@angular/common';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TagsComponent } from './draft-list/tags/tags.component';
import { LatestComponent } from './draft-list/latest/latest.component';
import { MostviewedComponent } from './draft-list/mostviewed/mostviewed.component';
import { MostfuckingComponent } from './draft-list/mostfucking/mostfucking.component';
import { DraftComponent } from './draft-list/draft/draft.component';
import { DraftFooterComponent } from './draft-list/draft-footer/draft-footer.component';
import { WebsitesComponent } from './draft-list/websites/websites.component';
import { SharedModule } from './shared/shared.module';
import { CorporateModule } from './corporate/corporate.module';
import { httpLoaderFactory } from './app.module';
import { AuthGuardService } from './services/Guards/auth-guard.service';
import { BackendApiService } from './services/backend-api.service';
import { AuthenticationApiService } from './services/authentication-api.service';
import { AppSettingsService } from './services/app-settings.service';
import { StorageService } from './services/storage.service';
import { AppSettingsDto } from './services/Dtos/AppSettingsDto';
import { TestBedHelper } from './testing/testbed.helper';

describe('AppComponent', () => {

  let fixture: ComponentFixture<AppComponent>;

  beforeEach(waitForAsync(() => {

    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        TagsComponent,
        LatestComponent,
        MostviewedComponent,
        MostfuckingComponent,
        DraftComponent,
        DraftFooterComponent,
        WebsitesComponent
      ],
      imports: [
        CommonModule,
        HttpClientTestingModule,
        BrowserModule,
        FormsModule,
        SharedModule,
        CorporateModule,
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
        BackendApiService,
        AuthGuardService,
        AppSettingsService,
        AuthenticationApiService,
        StorageService
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    TestBedHelper.setLanguage();
    fixture.detectChanges();
  });

  it('should create the app',
    waitForAsync(() => {
      const app = fixture.debugElement.componentInstance;
      expect(app).toBeTruthy();
  }));

});
