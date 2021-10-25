import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DraftDigestDto } from '../../services/Dtos/DraftDigestDto';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-latest',
  templateUrl: './latest.component.html',
  styleUrls: ['./latest.component.less']
})
export class LatestComponent implements OnInit {

  latest: DraftDigestDto[];
  take: number;

  constructor(private apiService: BackendApiService, private router: Router) {
    this.take = 5;
    this.latest = [];
   }

  ngOnInit() {
    this.apiService.getLatest(this.take).subscribe(l => this.latest = l.payload);
  }

  go(id: string) {
    this.router.navigate(['news', id]);
  }

}
