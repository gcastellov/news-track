import { Component, Input, OnInit } from '@angular/core';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { CommentDto } from 'src/app/services/Dtos/CommentDto';
import { StorageService } from 'src/app/services/storage.service';

@Component({
  selector: 'app-comment-entry',
  templateUrl: './comment-entry.component.html',
  styleUrls: ['./comment-entry.component.less']
})
export class CommentEntryComponent implements OnInit {

  @Input()
  comment: CommentDto | undefined;

  isLikeable: boolean;

  constructor(
    private _apiService: BackendApiService,
    private _storageService: StorageService) { 
      this.isLikeable = true;
    }

  ngOnInit(): void {
    if (this.comment) {
      this.isLikeable = this._storageService.getItem(this.comment.id) === null;
    }
  }

  like(): void {
    if (this.comment && this.isLikeable) {
      this._apiService.setLike(this.comment.id).subscribe(s => {
        if (this.comment) {
          this.comment.likes = s.payload.amount
          this._storageService.setItem(this.comment.id, this.comment.id);
          this.isLikeable = false;
        }
      });
    }
  }
}
