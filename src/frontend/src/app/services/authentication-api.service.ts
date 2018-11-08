import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { JwtHelper } from 'angular2-jwt';
import { Observable } from 'rxjs/Observable';
import { environment } from '../../environments/environment';
import { AuthenticationDto } from './Dtos/AuthenticationDto';
import { TokenResponseDto } from './Dtos/TokenResponseDto';
import { AuthenticationResult } from './Dtos/AuthenticationResult';
import { StorageService } from './storage.service';

@Injectable()
export class AuthenticationApiService {

    token: string;
    username: string;
    private _jwtHelper: JwtHelper;

    constructor(private _client: HttpClient, private _storageService: StorageService) {
        this._jwtHelper = new JwtHelper();
        this.token = this._storageService.getItem('token');
        if (this.token) {
            this.username = this._storageService.getItem('username');
        }
    }

    authenticate(auth: AuthenticationDto): Observable<AuthenticationResult> {
        const authUrl = `${environment.baseUrl}/api/authentication/generate`;
        return new Observable(observer => {
            const result = new AuthenticationResult();
            result.username = auth.username;
            this._client.post<TokenResponseDto>(authUrl, auth).subscribe(
                d => {
                    if (d.isSuccessful) {
                        this.setCredential(d.token, d.username);
                        result.token = d.token;
                        result.isSuccess = true;
                    } else {
                        result.failureReason = d.failure;
                    }

                    observer.next(result);
                },
                e => {
                    result.failureReason = -1;
                    observer.next(result);
                }
            );
        });
    }

    setCredential(token: string, username: string) {
        this.token = token;
        this.username = username;
        this._storageService.setItem('token', token);
        this._storageService.setItem('username', username);
    }

    isAuthenticated(): boolean {
        return this.token !== null && !this.isExpired();
    }

    getTokenHeaders(): HttpHeaders {
        return new HttpHeaders().set('Authorization', `Bearer ${this.token}`);
    }

    isExpired(): boolean {
        return this._jwtHelper.isTokenExpired(this.token);
    }

    isInRole(role: string): boolean {
        const identity = this._jwtHelper.decodeToken(this.token);
        const identityRole = identity['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        return identityRole === role;
    }
}
