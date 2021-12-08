import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DraftDto } from 'src/app/services/Dtos/DraftDto';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.less']
})
export class CommentComponent {

  @Input()
  draft: DraftDto | undefined;

  commentForm: FormGroup;

  constructor(fBuilder: FormBuilder) {
    this.commentForm = fBuilder.group({
      comment: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

}
