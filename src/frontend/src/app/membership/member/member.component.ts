import { Component, OnInit } from '@angular/core';
import { IdentityDto } from '../../services/Dtos/IdentityDto';
import { BackendApiService } from '../../services/backend-api.service';

@Component({
  selector: 'app-member',
  templateUrl: './member.component.html',
  styleUrls: ['./member.component.less']
})
export class MemberComponent implements OnInit {

  identity: IdentityDto | undefined;

  constructor(private _apiService: BackendApiService) { }

  ngOnInit() {
    this._apiService.getIdentity().subscribe(i => this.identity = i.payload);
  }

}
