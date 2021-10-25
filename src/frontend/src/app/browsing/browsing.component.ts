import { Component, OnInit } from '@angular/core';
import { BackendApiService } from '../services/backend-api.service';
import { Router } from '@angular/router';
import { NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { BrowsingDraft } from './browsing-draft/bowsing-draft';
import { debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-browsing',
  templateUrl: './browsing.component.html',
  styleUrls: ['./browsing.component.less']
})
export class BrowsingComponent implements OnInit {

  url: string;
  tags: string[];
  draft: BrowsingDraft;
  isForbidden: boolean;
  model: string;

  constructor(private _apiService: BackendApiService, private _router: Router, _draft: BrowsingDraft) {
    this.draft = _draft ? _draft : new BrowsingDraft(_apiService);
    this.tags = [];
    this.url = '';
    this.isForbidden = false;
    this.model = '';
  }

  ngOnInit(): void {
    const availableTags = this._apiService.getTags();
    availableTags.subscribe(data => this.tags = data.payload);
    if (this.draft.browseResult) {
      this.url = this.draft.browseResult.uri;
    }
  }

  onFind(): void {
    this.draft.initialize();
    this.draft.url = this.url;
    this._apiService.checkWebsite(this.draft.url).subscribe(r => {
      this.isForbidden = !r.payload.isAccepted;
    });
    this._apiService.browse(this.draft.url).subscribe(data => this.draft.setData(data.payload));
  }

  onSend(): void {
    this.draft.save().subscribe(() => this.url = this.draft.browseResult.uri);
  }

  onSetRelationship(): void {
    this._router.navigateByUrl('browsing/relationship');
  }

  isSendEnabled(): boolean {
    return this.draft && this.draft.isCompleted();
  }

  isFindEnabled(): boolean {
    return !!this.url;
  }

  onSearchItemSelected(event: NgbTypeaheadSelectItemEvent) {
    const item = event.item as string;
    this.addTag(item);
  }

  addNewTag() {
    this.addTag(this.model);
  }

  addTag(event: any) {
    if (this.draft.draftRequest && this.draft.draftRequest.tags.indexOf(event) < 0) {
      this.draft.draftRequest.tags.push(event);
    }
  }

  removeTag(event: string) {
    if (this.draft.draftRequest && this.draft.draftRequest.tags) {
      const index = this.draft.draftRequest.tags.indexOf(event);
      if (index > -1) {
        this.draft.draftRequest.tags.splice(index, 1);
      }
    }
  }

  getTags = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map(term => term.length < 2 ? []
        : this.tags.filter(v => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10)))
}
