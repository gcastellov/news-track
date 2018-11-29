import { Component, OnInit } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { DraftDto } from '../../services/Dtos/DraftDto';

@Component({
  selector: 'app-suggestions',
  templateUrl: './suggestions.component.html',
  styleUrls: ['./suggestions.component.less']
})
export class SuggestionsComponent implements OnInit {

  id: string;
  skip: number;
  take: number;
  draftIds: Observable<string[]>;
  ob: Observer<string[]>;
  drafts: DraftDto[];
  count: number;

  constructor(private _apiService: BackendApiService, private _route: ActivatedRoute) {
    this.take = 5;
    this.skip = 0;
    this.drafts = [];
    this.draftIds = new Observable<string[]>(observer => this.ob = observer);
    this.draftIds.subscribe(s => {
      s.forEach(id => {
        _apiService.getDraft(id).subscribe(d => this.drafts.push(d.payload));
      });
    });
  }

  ngOnInit() {
    this._route.params.subscribe(params => {
      this.id = params['id'];
      this.getSuggestions();
    });
  }

  onScroll() {
    if (this.count > this.drafts.length) {
      this.skip += this.take;
      this.getSuggestions();
    }
  }


  private getSuggestions() {
    this._apiService.getAllDraftSuggestions(this.id, this.take, this.skip).subscribe(s => {
      this.ob.next(s.payload.suggestedIds);
      this.count = s.payload.count;
    });
  }
}
