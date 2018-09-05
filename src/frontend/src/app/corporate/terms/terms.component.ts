import { Component, OnInit } from '@angular/core';
import { AppSettingsService } from '../../services/app-settings.service';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.less']
})
export class TermsComponent implements OnInit {

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
