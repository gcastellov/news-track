import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationModule } from './authentication/authentication.module';
import { CorporateModule } from './corporate/corporate.module';
import { DraftListModule } from './draft-list/draft-list.module';
import { SharedModule } from './shared/shared.module';
import { AuthenticationComponent } from './authentication/authentication/authentication.component';
import { AboutComponent } from './corporate/about/about.component';
import { PrivacyComponent } from './corporate/privacy/privacy.component';
import { TermsComponent } from './corporate/terms/terms.component';
import { DraftEntryComponent } from './draft-list/draft-entry/draft-entry.component';
import { DraftListComponent } from './draft-list/draft-list.component';
import { SearchComponent } from './draft-list/search/search.component';
import { SuggestionsComponent } from './draft-list/suggestions/suggestions.component';
import { SignupComponent } from './authentication/signup/signup.component';
import { CommentThreadComponent } from './comment/comment-thread/comment-thread.component';

const routes: Routes = [
  { path: 'latest', component: DraftListComponent },
  { path: 'most-viewed', component: DraftListComponent },
  { path: 'most-embarrasing', component: DraftListComponent },
  { path: 'search', component: SearchComponent },
  { path: '', redirectTo: 'latest', pathMatch: 'full' },
  { path: 'news/:id', component: DraftEntryComponent },
  { path: 'news/:id/suggestions', component: SuggestionsComponent },
  { path: 'authentication', component: AuthenticationComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'about', component: AboutComponent },
  { path: 'terms', component: TermsComponent },
  { path: 'privacy', component: PrivacyComponent },
  { path: 'membership',  loadChildren: () => import('./membership/membership.module').then(m => m.MembershipModule) },
  { path: 'browsing', loadChildren: () => import('./browsing/browsing.module').then(m => m.BrowsingModule) },
  { path: 'comment/:id', component: CommentThreadComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    DraftListModule,
    SharedModule,
    CorporateModule,
    AuthenticationModule,
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
