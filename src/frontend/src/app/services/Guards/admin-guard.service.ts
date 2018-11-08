import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthenticationApiService } from '../authentication-api.service';

@Injectable()
export class AdminGuardService implements CanActivate  {

    constructor(private _auth: AuthenticationApiService) {
    }

    canActivate(): boolean {
        return this._auth.isInRole('Administrator');
    }
}
