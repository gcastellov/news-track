import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../environments/environment';
import { AuthenticationDto } from './Dtos/AuthenticationDto';
import { TokenResponseDto } from './Dtos/TokenResponseDto';
import { AuthenticationResult } from './Dtos/AuthenticationResult';
import { StorageService } from './storage.service';
import { Envelope } from './Dtos/Envelope';
import { Observable } from 'rxjs';

const TokenKey = 'token';
const UsernameKey = 'username';

@Injectable()
export class AuthenticationApiService {

    token: string | undefined;
    username: string | undefined;
    private _jwtHelper: JwtHelperService;

    constructor(private _client: HttpClient, private _storageService: StorageService) {
        this.username = '';
        this._jwtHelper = new JwtHelperService();
        this.token = this._storageService.getItem(TokenKey) ?? undefined;
        if (this.token) {
            this.username = this._storageService.getItem(UsernameKey) ?? undefined;
        }
    }

    authenticate(auth: AuthenticationDto): Observable<AuthenticationResult> {
        const authUrl = `${environment.baseUrl}/api/authentication/generate`;
        return new Observable(observer => {
            const result = new AuthenticationResult();
            result.username = auth.username;
            this._client.post<Envelope<TokenResponseDto>>(authUrl, auth).subscribe(
                d => {
                    if (d.isSuccessful) {
                        this.setCredential(d.payload.token, d.payload.username);
                        result.token = d.payload.token;
                        result.isSuccess = true;
                    } else {
                        result.failureReason = d.payload.failure;
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
        this._storageService.setItem(TokenKey, token);
        this._storageService.setItem(UsernameKey, username);
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

    logout() {
        this.token = undefined;
        this.username = undefined;
        this._storageService.removeItem(TokenKey);
        this._storageService.removeItem(UsernameKey);
    }
}
