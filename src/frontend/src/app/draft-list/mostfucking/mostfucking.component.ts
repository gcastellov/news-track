import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DraftDigestDto } from '../../services/Dtos/DraftDigestDto';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-mostfucking',
  templateUrl: './mostfucking.component.html',
  styleUrls: ['./mostfucking.component.less']
})
export class MostfuckingComponent implements OnInit {

  mostFucking: DraftDigestDto[] = [];
  take: number;

  constructor(private apiService: BackendApiService, private router: Router) {
    this.take = 5;
  }

  ngOnInit() {
    this.apiService.getMostFucking(this.take).subscribe(l => this.mostFucking = l.payload);
  }

  go(id: string) {
    this.router.navigate(['news', id]);
  }

}
