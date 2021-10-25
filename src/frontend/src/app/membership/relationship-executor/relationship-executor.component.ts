import { Component } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-relationship-executor',
  templateUrl: './relationship-executor.component.html',
  styleUrls: ['./relationship-executor.component.less']
})
export class RelationshipExecutorComponent {

  isExecuted: boolean = false;

  constructor(private _apiService: BackendApiService) { }

  execute() {
    this._apiService.processSuggestions().subscribe(r => this.isExecuted = r);
  }

}
