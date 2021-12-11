import { Component, Input, OnInit } from '@angular/core';
import { CommentDto } from 'src/app/services/Dtos/CommentDto';

@Component({
  selector: 'app-comment-entry',
  templateUrl: './comment-entry.component.html',
  styleUrls: ['./comment-entry.component.less']
})
export class CommentEntryComponent implements OnInit {

  @Input()
  comment: CommentDto | undefined;

  constructor() { }

  ngOnInit(): void {
    if (this.comment !== undefined) {
      
    }
  }

}
