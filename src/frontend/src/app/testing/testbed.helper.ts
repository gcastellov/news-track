import { TestBed } from '@angular/core/testing';
import { HttpTestingController } from '@angular/common/http/testing';
import { TranslateService } from '@ngx-translate/core';

export class TestBedHelper {
    static setLanguage() {
        let http: HttpTestingController;
        let translate: TranslateService;

        translate = TestBed.get(TranslateService);
        http = TestBed.get(HttpTestingController);
        translate.use('en');
        http.expectOne('/assets/i18n/en.json');
        http.verify();
    }
}
