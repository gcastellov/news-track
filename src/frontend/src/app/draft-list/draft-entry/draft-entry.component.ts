import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BackendApiService } from '../../services/backend-api.service';
import { DraftDto } from '../../services/Dtos/DraftDto';
import { DraftDigestDto } from '../../services/Dtos/DraftDigestDto';
import { DraftSuggestionsDto } from '../../services/Dtos/DraftSuggestionsDto';
import { AuthenticationApiService } from 'src/app/services/authentication-api.service';
import { CreateCommentDto } from 'src/app/services/Dtos/CreateCommentDto';
import { CommentsListDto } from 'src/app/services/Dtos/CommentsListDto';

@Component({
  selector: 'app-draft-entry',
  templateUrl: './draft-entry.component.html',
  styleUrls: ['./draft-entry.component.less']
})
export class DraftEntryComponent implements OnInit {

  id: string;
  draft: DraftDto | undefined;
  relationship: DraftDigestDto[];
  suggestions: DraftSuggestionsDto | undefined;
  comments: CommentsListDto | undefined;
  commentsTake: number;
  commentsPage: number;
  
  private suggestionsTake: number;

  constructor(
    private _route: ActivatedRoute,
    private _router: Router,
    private _apiService: BackendApiService,
    private _authService: AuthenticationApiService) {
      this.id = '';
      this.relationship = [];
      this.suggestionsTake  = 5;
      this.commentsTake = 10;
      this.commentsPage = 0;
  }

  ngOnInit() {
    this._route.params.subscribe(params => {
      this.id = params['id'];
      this._apiService.setVisit(this.id).subscribe();
      this._apiService.getDraft(this.id).subscribe(s => this.draft = s.payload);
      this._apiService.getDraftRelationship(this.id).subscribe(s => this.relationship = s.payload);
      this._apiService.getDraftSuggestions(this.id, this.suggestionsTake).subscribe(s => this.suggestions = s.payload);
      this._apiService.getCommentsByDraftId(this.id, this.commentsTake, this.commentsPage).subscribe(c => this.comments = c.payload);
   });
  }

  go(id: string) {
    this._router.navigate(['news', id]);
  }

  suggest() {
    this._router.navigate([`news/${this.id}/suggestions`]);
  }

  searchByTag(tag: any) {
    this._router.navigate(['/search'], { queryParams: { tags: tag } });
  }

  canShowComment() {
    return this._authService.isAuthenticated();
  }

  createComment(commentDto: CreateCommentDto) {
    this._apiService.comment(commentDto).subscribe(s => {
      if (s.isSuccessful && this.commentsPage == 0) {
        let comments = [ s.payload ];
        if (this.comments) {
          this.comments.comments = comments.concat(this.comments.comments);
        } else {
          this.comments = new CommentsListDto();
          this.comments.comments = comments;
          this.comments.count = 1;
        }
      }
    });
  }

  onCommentsPageChanged(page: number) {
    this.commentsPage = page;
    this._apiService.getCommentsByDraftId(this.id, this.commentsTake, this.commentsPage).subscribe(c => this.comments = c.payload);
  }

}
