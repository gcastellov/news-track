import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DraftDigestDto } from '../../services/Dtos/DraftDigestDto';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-mostviewed',
  templateUrl: './mostviewed.component.html',
  styleUrls: ['./mostviewed.component.less']
})
export class MostviewedComponent implements OnInit {

  mostViewed: DraftDigestDto[];
  take: number;
  currentRoute: string;

  constructor(private _apiService: BackendApiService, private _router: Router) {
    this.mostViewed = [];
    this.take = 5;
    this.currentRoute = '';
   }

  ngOnInit() {
    this._apiService.getMostViewed(this.take).subscribe(l => this.mostViewed = l.payload);
  }

  go(id: string) {
    this._router.navigate(['news', id]);
  }

}
