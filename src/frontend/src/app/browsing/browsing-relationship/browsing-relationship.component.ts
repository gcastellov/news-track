import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BackendApiService } from '../../services/backend-api.service';
import { BrowsingDraft } from '../browsing-draft/bowsing-draft';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';

@Component({
  selector: 'app-browsing-relationship',
  templateUrl: './browsing-relationship.component.html',
  styleUrls: ['./browsing-relationship.component.less']
})
export class BrowsingRelationshipComponent implements OnInit {

  draft: BrowsingDraft;
  drafts: DraftDto[];
  count: number;
  page: number;
  take: number;
  numberOfPages: number;

  constructor(private _apiService: BackendApiService, private _router: Router, _draft: BrowsingDraft) {
    this.draft = _draft;
    this.page = 0;
    this.take = 5;
    this.numberOfPages = 0;
    this.drafts = [];
    this.count = 0;
  }

  ngOnInit() {
    this.getDrafts();
  }

  getDrafts() {
    this._apiService.getLatestDrafts(this.page, this.take).subscribe(d => {
      this.count = d.payload.count;
      this.drafts = d.payload.news;
      this.numberOfPages = this.count / this.take;
    });
  }

  onPageChange(event: number) {
    this.page = event;
    this.getDrafts();
  }

  onSelectionChange(event: DraftRelationshipDto) {
    const id = event.id;
    const index = this.draft.relationship.findIndex(r => r.id === id);
    if (index < 0) {
      this.draft.relationship.push(event);
    } else {
      this.draft.relationship.splice(index, 1);
    }
  }

  onBack() {
    this._router.navigateByUrl('/browsing');
  }

  onSend(): void {
    this.draft.save().subscribe(r => {
      if (r) {
        this._router.navigateByUrl('/browsing');
      }
    });
  }

  isSendEnabled(): boolean {
    return this.draft && this.draft.isCompleted();
  }

  isDraftSelected(draft: DraftDto): boolean {
    const index = this.draft.relationship.findIndex(r => r.id === draft.id);
    return index > -1;
  }

}
