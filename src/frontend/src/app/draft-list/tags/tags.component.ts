import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BackendApiService } from '../../services/backend-api.service';
import { TagScore } from '../../services/Dtos/TagsStatsResponseDto';
import { Tag } from './tag';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.less']
})
export class TagsComponent implements OnInit {

  @Output()
  selectionChange: EventEmitter<string> = new EventEmitter<string>();

  tags: Tag[];
  averageScore: number;

  constructor(private _apiService: BackendApiService) {
    this.tags = [];
    this.averageScore = 0;
  }

  ngOnInit() {
    this._apiService.getStatsTags().subscribe(
      t => {
        this.tags = [];
        if (t.isSuccessful && t.payload.tagsScore) {
          this.averageScore = t.payload.averageScore;
          this.calcScores(t.payload.tagsScore);
        }
      });
  }

  onSelected(tag: Tag) {
    this.selectionChange.emit(tag.name);
  }

  calcScores(tagScores: TagScore[]) {
    const overAverage = tagScores.filter(t => t.score >= this.averageScore);
    const factor = overAverage.map(t => t.score).reduce((sum, current) => sum + current);

    tagScores.forEach(t => {

      const tag = new Tag(t.tag, t.score);
      if (t.score > this.averageScore) {
        const percent = t.score / factor;
        if (percent > 0.8) {
          tag.importance = 5;
        } else if (percent > 0.6) {
          tag.importance = 4;
        } else if (percent > 0.4) {
          tag.importance = 3;
        } else if (percent > 0.2) {
          tag.importance = 2;
        } else {
          tag.importance = 1;
        }
      }

      this.tags.push(tag);
    });

    this.tags = this.shuffle(this.tags);
  }

  shuffle(array: any) {
    let currentIndex = array.length;
    let temporaryValue;
    let randomIndex;
    while (0 !== currentIndex) {
      randomIndex = Math.floor(Math.random() * currentIndex);
      currentIndex -= 1;
      temporaryValue = array[currentIndex];
      array[currentIndex] = array[randomIndex];
      array[randomIndex] = temporaryValue;
    }
    return array;
  }

}
