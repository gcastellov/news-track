import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommentsListDto } from 'src/app/services/Dtos/CommentsListDto';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.less']
})
export class CommentListComponent implements OnInit {

  @Output()
  paginated: EventEmitter<number> = new EventEmitter<number>();
  
  @Input()
  comments: CommentsListDto | undefined;
 
  @Input()
  page: number;

  @Input()
  take: number;
  
  numberOfPages: number;

  constructor() {
    this.page = 0;
    this.take = 0;
    this.numberOfPages = 0;
  }

  ngOnInit(): void {
    if (this.comments) {
      this.numberOfPages = Math.floor(this.comments.count / this.take);
    }
  }

  onPageChange(page: number): void {
    this.page = page;
    this.paginated.emit(page);
  }

}
