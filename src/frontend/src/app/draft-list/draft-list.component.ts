import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbTypeaheadSelectItemEvent } from '@ng-bootstrap/ng-bootstrap';
import { BackendApiService } from '../services/backend-api.service';
import { DraftDto } from '../services/Dtos/DraftDto';
import { SearchResultDto } from '../services/Dtos/SearchResultDto';
import { debounceTime, distinctUntilChanged, switchMap, map, catchError, tap, mergeAll } from "rxjs/operators";
import { DraftListDto } from '../services/Dtos/DraftListDto';
import { Envelope } from '../services/Dtos/Envelope';
import { merge, Observable, of } from 'rxjs';

@Component({
  selector: 'app-draft-list',
  templateUrl: './draft-list.component.html',
  styleUrls: ['./draft-list.component.less']
})
export class DraftListComponent implements OnInit {

  drafts: DraftDto[] | undefined;
  count: number;
  page: number;
  take: number;
  numberOfPages: number;
  model: any;
  searching = false;
  searchFailed = false;
  currentRoute: string;
  errorMessage: string | null;

  constructor(public _apiService: BackendApiService, private _router: Router, private _activatedRoute: ActivatedRoute) {
    this.count = 0;
    this.numberOfPages = 0;
    this.currentRoute = '';
    this.take = 5;
    this.page = 0;
    this.errorMessage = null;
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

  onTagSelected(event: any) {
    if (event) {
      this._router.navigate(['/search'], { queryParams: { tags: event } });
    }
  }

  onWebsiteSelected(event: any) {
    if (event) {
      this._router.navigate(['/search'], { queryParams: { website: event } });
    }
  }

  search = (text$: Observable<string>) => 
    text$
    .pipe(
      debounceTime<string>(300),
      distinctUntilChanged<string>(),
      switchMap(term => {
        return this._apiService.search(term).pipe(
          tap(r => this.searchFailed = false),
          map(r => r.payload),
          catchError(() => {
            this.searchFailed = true;
            return of([]);
          })
        )
      }),
      tap(r => this.searching = false)
    )

  private loadDrafts(response: Envelope<DraftListDto>) {
    this.count = 0;
    this.drafts = [];
    this.errorMessage = '';
    if (response.isSuccessful) {
      this.count = response.payload.count;
      this.drafts = response.payload.news;
      this.numberOfPages = this.count / this.take;
    } else if (response.error) {
      this.errorMessage = response.error.message;
    }
  }

  private setDefaultErrorMessage(error: any) {
    this.errorMessage = `Impossible to load. ${error.statusText}`;
  }

}
