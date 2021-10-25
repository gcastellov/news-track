import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { WebsiteStatsDto } from '../../services/Dtos/WebsiteStatsDto';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-websites',
  templateUrl: './websites.component.html',
  styleUrls: ['./websites.component.less']
})
export class WebsitesComponent implements OnInit {

  @Output()
  selectionChange: EventEmitter<string> = new EventEmitter<string>();

  websites: WebsiteStatsDto[];
  take: number;

  constructor(private _apiService: BackendApiService) {
    this.websites = [];
    this.take = 5;
   }

  ngOnInit() {
    this._apiService.getWebsites(this.take).subscribe(w => this.websites = w.payload);
  }

  onSelected(website: string) {
    this.selectionChange.emit(website);
  }
}
