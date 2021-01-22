import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { BackendApiService } from '../services/backend-api.service';
import { DraftDto } from '../services/Dtos/DraftDto';
import { SearchResultDto } from '../services/Dtos/SearchResultDto';
import { Observable } from 'rxjs/Observable';
import {of} from 'rxjs/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/merge';
import { DraftListDto } from '../services/Dtos/DraftListDto';
import { Envelope } from '../services/Dtos/Envelope';

@Component({
  selector: 'app-draft-list',
  templateUrl: './draft-list.component.html',
  styleUrls: ['./draft-list.component.less']
})
export class DraftListComponent implements OnInit {

  drafts: DraftDto[];
  count: number;
  page: number;
  take: number;
  numberOfPages: number;
  hits: SearchResultDto[];
  model: any;
  searching = false;
  searchFailed = false;
  hideSearchingWhenUnsubscribed = new Observable(() => () => this.searching = false);
  currentRoute: string;
  errorMessage: string = null;

  constructor(public _apiService: BackendApiService, private _router: Router, private _activatedRoute: ActivatedRoute) {
    this.take = 5;
    this.page = 0;
    this._activatedRoute.url.subscribe(url => this.currentRoute = url[0].path);
  }

  ngOnInit() {
    this.getDrafts();
  }

  getDrafts() {
    switch (this.currentRoute) {
      case 'latest':
        this.getLatest();
        break;
      case 'most-viewed':
        this.getMostViewed();
        break;
      case 'most-embarrasing':
        this.getMostFucked();
        break;
    }
  }

  getLatest() {
    this._apiService.getLatestDrafts(this.page, this.take).subscribe(
      d => this.loadDrafts(d),
      e => this.setDefaultErrorMessage(e));
  }

  getMostViewed() {
    this._apiService.getMostViewedDrafts(this.page, this.take).subscribe(
      d => this.loadDrafts(d),
      e => this.setDefaultErrorMessage(e));
  }

  getMostFucked() {
    this._apiService.getMostFuckedDrafts(this.page, this.take).subscribe(
      d => this.loadDrafts(d),
      e => this.setDefaultErrorMessage(e));
  }

  onPageChange(event: number) {
    this.page = event;
    this.getDrafts();
  }

  onSearchItemSelected(event: NgbTypeaheadSelectItemEvent) {
    const item = event.item as SearchResultDto;
    this._router.navigate(['news', item.id]);
  }

  onTagSelected(event) {
    if (event) {
      this._router.navigate(['/search'], { queryParams: { tags: event } });
    }
  }

  onWebsiteSelected(event) {
    if (event) {
      this._router.navigate(['/search'], { queryParams: { website: event } });
    }
  }

  search = (text$: Observable<string>) =>
    text$
    .debounceTime(300)
    .distinctUntilChanged()
    .do(() => this.searching = true)
    .switchMap(term =>
      this._apiService.search(term)
        .do(() => this.searchFailed = false)
        .map(r => r.payload)
        .catch(() => {
          this.searchFailed = true;
          return of([]);
        }))
    .do(() => this.searching = false)
    .merge(this.hideSearchingWhenUnsubscribed)

  private loadDrafts(response: Envelope<DraftListDto>) {
    this.count = 0;
    this.drafts = null;
    this.errorMessage = null;
    if (response.isSuccessful) {
      this.count = response.payload.count;
      this.drafts = response.payload.news;
      this.numberOfPages = this.count / this.take;
    } else {
      this.errorMessage = response.errorMessage;
    }
  }

  private setDefaultErrorMessage(error: any) {
    this.errorMessage = "Impossible to load. " + error.statusText;
  }

}
