import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.less']
})
export class PaginatorComponent implements OnInit {

  @Input()
  page: number;

  @Input()
  numberOfPages: number;

  pages: number[];

  @Output()
  pageChange: EventEmitter<number> = new EventEmitter<number>();

  constructor() {
    this.page = 0;
    this.pages = [];
    this.numberOfPages = 0;
  }

  ngOnInit() {
    for (let i = 0; i < this.numberOfPages; i++) {
      this.pages.push(i);
    }
  }

  OnChanges() {
  }

  onPageChange(event: number) {
    this.page = event;
    this.pageChange.emit(this.page);
  }

  onNext() {
    if (this.canGoNext()) {
      this.page++;
      this.pageChange.emit(this.page);
    }
  }

  onPrevious() {
    if (this.canGoPrevious()) {
      this.page--;
      this.pageChange.emit(this.page);
    }
  }

  canGoNext(): boolean {
    return this.pages.length > this.page + 1;
  }

  canGoPrevious(): boolean {
    return this.page > 0;
  }

}
