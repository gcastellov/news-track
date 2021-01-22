import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from './shared/shared.module';
import { CorporateModule } from './corporate/corporate.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

import { AppComponent } from './app.component';
import { DraftListComponent } from './draft-list/draft-list.component';
import { DraftEntryComponent } from './draft-list/draft-entry/draft-entry.component';
import { SearchComponent } from './draft-list/search/search.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthenticationApiService } from './services/authentication-api.service';
import { BackendApiService } from './services/backend-api.service';
import { AuthenticationModule } from './authentication/authentication.module';
import { AuthGuardService } from './services/Guards/auth-guard.service';
import { AppSettingsService } from './services/app-settings.service';
import { AboutComponent } from './corporate/about/about.component';
import { TermsComponent } from './corporate/terms/terms.component';
import { PrivacyComponent } from './corporate/privacy/privacy.component';
import { StorageService } from './services/storage.service';
import { AuthInterceptor } from './services/Interceptors/auth-interceptor';
import { SuggestionsComponent } from './draft-list/suggestions/suggestions.component';
import { DraftListModule } from './draft-list/draft-list.module';
import { AdminGuardService } from './services/Guards/admin-guard.service';

export function httpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

export function configServiceFactory(config: AppSettingsService) {
  return () => config.initialize();
}

export function authInterceptorFactory (router: Router) {
  return new AuthInterceptor(router);
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    FormsModule,
    DraftListModule,
    InfiniteScrollModule,
    SharedModule,
    CorporateModule,
    AuthenticationModule,
    NgbModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: httpLoaderFactory,
          deps: [HttpClient]
      }
    }),
    RouterModule.forRoot([
      { path: 'latest', component: DraftListComponent },
      { path: 'most-viewed', component: DraftListComponent },
      { path: 'most-embarrasing', component: DraftListComponent },
      { path: 'search', component: SearchComponent },
      { path: '', redirectTo: 'latest', pathMatch: 'full' },
      { path: 'news/:id', component: DraftEntryComponent },
      { path: 'news/:id/suggestions', component: SuggestionsComponent },
      { path: 'authentication', component: AuthenticationComponent },
      { path: 'about', component: AboutComponent },
      { path: 'terms', component: TermsComponent },
      { path: 'privacy', component: PrivacyComponent },
      { path: 'membership', loadChildren: './membership/membership.module#MembershipModule' },
      { path: 'browsing', loadChildren: './browsing/browsing.module#BrowsingModule' }
    ])
  ],
  providers: [
    { provide: APP_INITIALIZER, useFactory: configServiceFactory, deps: [AppSettingsService], multi: true },
    { provide: HTTP_INTERCEPTORS, useFactory: authInterceptorFactory, multi: true, deps: [Router] },
    BackendApiService,
    AuthenticationApiService,
    AuthGuardService,
    AdminGuardService,
    AppSettingsService,
    StorageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
