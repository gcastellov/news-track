import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from './shared/shared.module';
import { CorporateModule } from './corporate/corporate.module';
import { AppComponent } from './app.component';
import { AuthenticationApiService } from './services/authentication-api.service';
import { BackendApiService } from './services/backend-api.service';
import { AuthGuardService } from './services/Guards/auth-guard.service';
import { AppSettingsService } from './services/app-settings.service';
import { StorageService } from './services/storage.service';
import { AuthInterceptor } from './services/Interceptors/auth-interceptor';
import { AdminGuardService } from './services/Guards/admin-guard.service';
import { AppRoutingModule } from './app-routing.module';
import { InitService } from './services/init.service';

export function httpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
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
    AppRoutingModule,  
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
  ],
  providers: [
    BackendApiService,
    AuthenticationApiService,
    AuthGuardService,
    AdminGuardService,
    AppSettingsService,
    StorageService,
    InitService,
    TranslateService,
    { provide: APP_INITIALIZER, useFactory: (initService:InitService)=>()=>initService.init(), deps: [InitService, AppSettingsService], multi: true },
    { provide: HTTP_INTERCEPTORS, useFactory: authInterceptorFactory, multi: true, deps: [Router] },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
