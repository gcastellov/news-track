import { Component } from '@angular/core';
import { StorageService } from '../../services/storage.service';

@Component({
  selector: 'app-cookie-consent',
  templateUrl: './cookie-consent.component.html',
  styleUrls: ['./cookie-consent.component.less']
})
export class CookieConsentComponent {

  isAccepted: boolean;
  private _cookieIsAccepted = 'cookieIsAccepted';

  constructor(private _storageService: StorageService) {
    this.isAccepted = Boolean(_storageService.getItem(this._cookieIsAccepted));
  }

  accept() {
    this.isAccepted = true;
    this._storageService.setItem(this._cookieIsAccepted, this.isAccepted.toString());
  }

}
