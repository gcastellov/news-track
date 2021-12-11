import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentComponent } from './comment/comment.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommentEntryComponent } from './comment-entry/comment-entry.component';
import { CommentListComponent } from './comment-list/comment-list.component';



@NgModule({
  declarations: [
    CommentComponent,
    CommentEntryComponent,
    CommentListComponent
  ],
  imports: [
    CommonModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
  ],
  exports: [CommentComponent, CommentListComponent]
})
export class CommentModule { }
