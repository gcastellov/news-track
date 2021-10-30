import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AuthenticationApiService } from './services/authentication-api.service';
import { AppSettingsService } from './services/app-settings.service';
import { AppSettingsDto } from './services/Dtos/AppSettingsDto';
import { Router } from '@angular/router';

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
    private _router: Router,
    authenticationService: AuthenticationApiService
  ) {
    this.authentication = authenticationService;
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

  logout() {
    this.authentication.logout();
    this._router.navigateByUrl('/authentication');
  }
}
