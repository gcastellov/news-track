import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthenticationApiService } from '../authentication-api.service';

@Injectable()
export class AuthGuardService implements CanActivate {

  constructor(private _auth: AuthenticationApiService, private _router: Router) {
  }

  canActivate(): boolean {
    if (!this._auth.isAuthenticated()) {
      this._router.navigateByUrl('/authentication');
      return false;
    }
    return true;
  }
}
