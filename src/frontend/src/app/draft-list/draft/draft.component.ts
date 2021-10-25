import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { BackendApiService } from '../../services/backend-api.service';
import { StorageService } from '../../services/storage.service';
import { AppSettingsService } from '../../services/app-settings.service';

@Component({
  selector: 'app-draft',
  templateUrl: './draft.component.html',
  styleUrls: ['./draft.component.less']
})
export class DraftComponent {

  expression: string;

  @Input()
  draft: DraftDto | undefined;

  @Input()
  isExtended: boolean;

  constructor(
    private _router: Router,
    private _apiService: BackendApiService,
    private _storageService: StorageService,
    private _settingsService: AppSettingsService
  ) {
    this.isExtended = false;
    this.expression = '';
    this._settingsService.getExpressions().subscribe(e =>
      this.expression = this.getExpression(e)
    );
  }

  go(id: string) {
    this._router.navigate(['news', id]);
  }

  isFucked(): boolean {
    if (this.draft) {
      const isFucked = this._storageService.getItem(this.draft.id);
      return isFucked !== null; 
    }
    return false;
  }

  fuck() {
    if (this.draft) {
      this._storageService.setItem(this.draft.id, this.draft.id);
      this._apiService.setFuck(this.draft.id).subscribe(f => { 
        if (this.draft)
          this.draft.fucks = f.payload.amount;
      });
    }
  }

  getExpression(expresssions: string[]): string {
    const i = Math.floor(Math.random() * (expresssions.length - 1));
    return expresssions[i];
  }
}
