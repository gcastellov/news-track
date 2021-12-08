import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { CommentModule } from '../comment/comment.module';
import { DraftListComponent } from './draft-list.component';
import { TagsComponent } from './tags/tags.component';
import { LatestComponent } from './latest/latest.component';
import { DraftComponent } from './draft/draft.component';
import { DraftEntryComponent } from './draft-entry/draft-entry.component';
import { SearchComponent } from './search/search.component';
import { WebsitesComponent } from './websites/websites.component';
import { MostviewedComponent } from './mostviewed/mostviewed.component';
import { DraftFooterComponent } from './draft-footer/draft-footer.component';
import { MostfuckingComponent } from './mostfucking/mostfucking.component';
import { SuggestionsComponent } from './suggestions/suggestions.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    CommentModule,
    RouterModule,
    NgbModule,
    TranslateModule,
    InfiniteScrollModule
  ],
  declarations: [
    DraftListComponent,
    TagsComponent,
    LatestComponent,
    DraftComponent,
    DraftEntryComponent,
    SearchComponent,
    WebsitesComponent,
    MostviewedComponent,
    DraftFooterComponent,
    MostfuckingComponent,
    SuggestionsComponent],
  exports: [
    DraftListComponent,
    TagsComponent,
    LatestComponent,
    DraftComponent,
    DraftEntryComponent,
    SearchComponent,
    WebsitesComponent,
    MostviewedComponent,
    DraftFooterComponent,
    MostfuckingComponent,
    SuggestionsComponent
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DraftListModule { }
