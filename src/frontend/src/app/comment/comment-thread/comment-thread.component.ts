import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationApiService } from 'src/app/services/authentication-api.service';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { CommentDto } from 'src/app/services/Dtos/CommentDto';
import { CommentsListDto } from 'src/app/services/Dtos/CommentsListDto';
import { CreateCommentDto } from 'src/app/services/Dtos/CreateCommentDto';

@Component({
  selector: 'app-comment-thread',
  templateUrl: './comment-thread.component.html',
  styleUrls: ['./comment-thread.component.less']
})
export class CommentThreadComponent implements OnInit {

  take: number;  
  page: number;
  comment: CommentDto | undefined;
  comments: CommentsListDto | undefined;

  constructor(
    private _route: ActivatedRoute,
    private _apiService: BackendApiService,
    private _authService: AuthenticationApiService) { 
      this.take = 10;
      this.page = 0;
  }

  ngOnInit(): void {
    this._route.params.subscribe(params => {
      let commentId =  params['id'];
      this._apiService.getComment(commentId).subscribe(c => this.comment = c.payload);
      this._apiService.getCommentsByCommentId(commentId, this.take, this.page).subscribe(c => this.comments = c.payload);
    });
  }

  onCommentsPageChanged(page: number) {

  }

  createComment(createdComment: CreateCommentDto) {
    this._apiService.comment(createdComment).subscribe(s => {
      let comments = [ s.payload ];
        if (this.comments) {
          this.comments.comments = comments.concat(this.comments.comments);
        } else {
          this.comments = new CommentsListDto();
          this.comments.comments = comments;
        }
        
        this.comments.count++;
        if (this.comment)
          this.comment.replies++;
    });
  }

  canShowComment() : boolean {
    return this._authService.isAuthenticated();
  }

}
