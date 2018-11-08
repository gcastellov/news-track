import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationApiService } from '../../services/authentication-api.service';
import { AdminGuardService } from '../../services/Guards/admin-guard.service';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.less']
})
export class MembershipComponent {

  username: string;
  adminGuard: AdminGuardService;

  constructor(
    private _router: Router,
    authSerive: AuthenticationApiService,
    adminGuard: AdminGuardService) {
      this.username = authSerive.username;
      this.adminGuard = adminGuard;
  }

  isRouteActive(route: string): boolean {
    return this._router.url.endsWith(route);
  }

}
