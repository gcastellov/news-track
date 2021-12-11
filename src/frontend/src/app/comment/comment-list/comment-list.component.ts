import { Component, Input, OnInit } from '@angular/core';
import { BackendApiService } from 'src/app/services/backend-api.service';
import { CommentDto } from 'src/app/services/Dtos/CommentDto';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.less']
})
export class CommentListComponent implements OnInit {
  
  @Input()
  draftId: string | undefined;
  
  comments: CommentDto[];
  skip: number;
  take: number;

  constructor(private _apiService: BackendApiService) {
    this.comments = [];
    this.skip = 0;
    this.take = 10;
  }

  ngOnInit(): void {
    if (this.draftId) {
      this._apiService.getComments(this.draftId, this.take, this.skip).subscribe(env => {
        if (env.isSuccessful) {
          this.comments = env.payload;
        }
      });
    }
  }

}
