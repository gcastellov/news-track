import { Component, Input, OnInit } from '@angular/core';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { CommentDto } from 'src/app/services/Dtos/CommentDto';
import { CommentsListDto } from 'src/app/services/Dtos/CommentsListDto';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.less']
})
export class CommentListComponent {
  
  @Input()
  comments: CommentsListDto;
 
  skip: number;
  take: number;

  constructor() {
    this.skip = 0;
    this.take = 10;
    this.comments = new CommentsListDto();
  }

}
