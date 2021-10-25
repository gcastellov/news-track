import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HttpHandler, HttpEvent, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private _router: Router) { }

    private handleError(req: HttpRequest<any>, err: HttpErrorResponse): Observable<any> {
        if (err.status === 401 || err.status === 403) {
            this._router.navigateByUrl('authentication');
            return new Observable(observer => observer.next(err.message));
        }
        if (!environment.production) {
            console.log('Exception from: ' + req.url);
            console.log(err);
        }
        return throwError(err);
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req)
            .pipe(catchError(error => this.handleError(req, error)));
    }
}
