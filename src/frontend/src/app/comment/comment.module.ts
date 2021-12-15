import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentComponent } from './comment/comment.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommentEntryComponent } from './comment-entry/comment-entry.component';
import { CommentListComponent } from './comment-list/comment-list.component';
import { SharedModule } from '../shared/shared.module';
import { CommentThreadComponent } from './comment-thread/comment-thread.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    CommentComponent,
    CommentEntryComponent,
    CommentListComponent,
    CommentThreadComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    TranslateModule,
    SharedModule
  ],
  exports: [CommentComponent, CommentListComponent, CommentThreadComponent]
})
export class CommentModule { }
