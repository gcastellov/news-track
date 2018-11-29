import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BackendApiService } from '../../services/backend-api.service';
import { DraftDto } from '../../services/Dtos/DraftDto';
import 'rxjs/add/operator/filter';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.less']
})
export class SearchComponent implements OnInit {

  pattern: string;
  page: number;
  take: number;
  count: number;
  tags: string[];
  website: string;
  numberOfPages: number;
  drafts: DraftDto[];
  errorMessage: string = null;

  constructor(private _apiService: BackendApiService, private _route: ActivatedRoute, private _router: Router) {
    this.take = 5;
    this.page = 0;
    this.tags = [];
    this.pattern = '';
    this.website = '';
  }

  ngOnInit() {
    this.setAndSearch();
  }

  setAndSearch() {
    this._route.queryParams.subscribe(params => {
      const tags = params['tags'];
      const website = params['website'];
      if (website) {
        this.website = website;
      }
      if (tags) {
        this.tags = tags.split(',');
      }

      this.search();
    });
  }

  search() {
    this._apiService.advancedSearch(this.website, this.pattern, this.tags, this.page, this.take).subscribe(d => {
      this.count = 0;
      this.drafts = null;
      this.errorMessage = null;
      if (d.isSuccessful) {
        this.count = d.payload.count;
        this.drafts = d.payload.news;
        this.numberOfPages = this.count / this.take;
      } else {
        this.errorMessage = d.errorMessage;
      }
    });
  }

  removeTag(tag: string) {
    this.onTagSelected(tag);
  }

  onTagSelected(event) {
    const index = this.tags.indexOf(event);
    let pTags: string[] = this.tags;
    if (index > -1) {
      pTags.splice(index, 1);
    } else {
      pTags = pTags.concat(event);
    }
    const params = this.createQueryParams(pTags, this.website);
    this._router.navigate(['/search'], { queryParams: params });
  }

  onWebsiteSelected(event) {
    if (event) {
      const params = this.createQueryParams(this.tags, event);
      this._router.navigate(['/search'], { queryParams: params });
    }
  }

  onSearch() {
    this.search();
  }

  onPageChange(event) {
    this.page = event;
    this.search();
  }

  private createQueryParams(tags: string[], webstite: string): any {
    const strTags = this.plainTags(tags);
    const params = { website: null, tags: strTags };
    if (webstite) {
      params.website = webstite;
    }
    return params;
  }

  private plainTags(tags: string[]): string {
      let strTags = tags[0];
      for (let i = 1; i < tags.length; i++) {
        strTags += `,${tags[i]}`;
      }
      return strTags;
  }
}
