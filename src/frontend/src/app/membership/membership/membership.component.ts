import { Component } from '@angular/core';
import { AuthenticationApiService } from '../../services/authentication-api.service';
import { AdminGuardService } from '../../services/Guards/admin-guard.service';

@Component({
  selector: 'app-membership',
  templateUrl: './membership.component.html',
  styleUrls: ['./membership.component.less']
})
export class MembershipComponent {

  username: string;
  isAdmin: boolean;

  constructor(authSerive: AuthenticationApiService, adminGuard: AdminGuardService) {
      this.username = authSerive.username ?? '';
      this.isAdmin = adminGuard.canActivate();
  }
}
