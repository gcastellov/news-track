import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { AboutComponent } from './about/about.component';
import { TermsComponent } from './terms/terms.component';
import { PrivacyComponent } from './privacy/privacy.component';
import { CookieConsentComponent } from './cookie-consent/cookie-consent.component';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule
  ],
  exports: [CookieConsentComponent],
  declarations: [AboutComponent, TermsComponent, PrivacyComponent, CookieConsentComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CorporateModule { }
