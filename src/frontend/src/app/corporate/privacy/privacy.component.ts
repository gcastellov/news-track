import { Component, OnInit } from '@angular/core';
import { AppSettingsService } from '../../services/app-settings.service';

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.less']
})
export class PrivacyComponent implements OnInit {

  corporation: any;

  constructor(private _settingsService: AppSettingsService) {
    this.corporation = { company: '', country: '' };
  }

  ngOnInit() {
    this._settingsService.getSettings().subscribe(s =>
      this.corporation = {
        company: s.corporation,
        country: s.country
      }
    );
  }

}
