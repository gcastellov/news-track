import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationApiService } from './services/authentication-api.service';
import { AppSettingsService } from './services/app-settings.service';
import { AppSettingsDto } from './services/Dtos/AppSettingsDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
  title = 'NewsTrack';
  authentication: AuthenticationApiService;
  isCollapsed = true;
  year: number;
  settings: AppSettingsDto | undefined;

  constructor(
    private _translate: TranslateService,
    private _appSettings: AppSettingsService,
    _authenticationService: AuthenticationApiService
  ) {
    this.authentication = _authenticationService;
    this.year = new Date().getFullYear();
  }

  ngOnInit() {
    this._appSettings.getSettings().subscribe(settings => {
      this.settings = settings;
    });
  }

  toggleMenu() {
    this.isCollapsed = !this.isCollapsed;
  }

  hasSocial(): boolean {
    return !!this.settings?.facebookUrl || !!this.settings?.githubUrl || !!this.settings?.twitterUrl;
  }

  changeLangugage(lang: string) {
    this._translate.use(lang);
  }
}
