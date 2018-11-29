import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BackendApiService } from '../../services/backend-api.service';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { DraftDigestDto } from '../../services/Dtos/DraftDigestDto';
import { DraftSuggestionsDto } from '../../services/Dtos/DraftSuggestionsDto';

@Component({
  selector: 'app-draft-entry',
  templateUrl: './draft-entry.component.html',
  styleUrls: ['./draft-entry.component.less']
})
export class DraftEntryComponent implements OnInit {

  id: string;
  draft: DraftDto;
  relationship: DraftDigestDto[];
  suggestions: DraftSuggestionsDto;
  private take: number;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _apiService: BackendApiService) {
      this.take  = 5;
  }

  ngOnInit() {
    this._route.params.subscribe(params => {
      this.id = params['id'];
      this._apiService.setVisit(this.id).subscribe();
      this._apiService.getDraft(this.id).subscribe(s => this.draft = s.payload);
      this._apiService.getDraftRelationship(this.id).subscribe(s => this.relationship = s.payload);
      this._apiService.getDraftSuggestions(this.id, this.take).subscribe(s => this.suggestions = s.payload);
   });
  }

  go(id: string) {
    this._router.navigate(['news', id]);
  }

  suggest() {
    this._router.navigate([`news/${this.id}/suggestions`]);
  }

  searchByTag(tag) {
    this._router.navigate(['/search'], { queryParams: { tags: tag } });
  }

}
