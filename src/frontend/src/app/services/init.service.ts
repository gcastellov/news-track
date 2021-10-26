import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Observable, of } from "rxjs";
import { AppSettingsService } from './app-settings.service';
import { AppSettingsDto } from './Dtos/AppSettingsDto';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  constructor(private _appSettings: AppSettingsService, private _translate: TranslateService) {     
  }

  init() {
    return new Observable((subscriber) => {
      this._appSettings.initialize().then(() => {
            const settings = this._appSettings.settings ?? new AppSettingsDto();
            this._translate.setDefaultLang(settings.defaultLanguage);
            this._translate.use(settings.defaultLanguage);
            subscriber.next();
            subscriber.complete();
        });
    });
  }
}