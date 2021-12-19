import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateCommentDto } from 'src/app/services/Dtos/CreateCommentDto';
import { DraftDto } from 'src/app/services/Dtos/DraftDto';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.less']
})
export class CommentComponent {

  @Input()
  draftId: string | undefined;

  @Input()
  replyingTo: string | undefined;

  @Output()
  eventSent: EventEmitter<CreateCommentDto> = new EventEmitter<CreateCommentDto>();

  commentForm: FormGroup;

  constructor(fBuilder: FormBuilder) {
    this.commentForm = fBuilder.group({
      comment: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  send() {    
    if (this.draftId) {
      const content: string = this.commentForm.get('comment')?.value;
      const commentDto = new CreateCommentDto(this.draftId, content);
      if (this.replyingTo !== undefined) {
        commentDto.replyingTo = this.replyingTo;
      }

      this.commentForm.setValue({comment: ''})
      this.eventSent.emit(commentDto);
    }
  }

}
