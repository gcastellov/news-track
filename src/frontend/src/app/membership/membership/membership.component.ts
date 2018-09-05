import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationApiService } from '../../services/authentication-api.service';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.less']
})
export class MembershipComponent implements OnInit {

  username: string;

  constructor(private _router: Router, _authSerive: AuthenticationApiService) {
    this.username = _authSerive.username;
  }

  ngOnInit() {
  }

  isRouteActive(route: string): boolean {
    return this._router.url.endsWith(route);
  }

}
