import { Injectable } from '@angular/core';
import { AppSettingsDto } from './Dtos/AppSettingsDto';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable()
export class AppSettingsService {

  settings: AppSettingsDto | undefined;
  expressions: string[] | undefined;

  constructor(private http: HttpClient) {
  }

  getSettings(): Observable<AppSettingsDto> {

    if (this.settings) {
      return new Observable<AppSettingsDto>(observer => {
        observer.next(this.settings);
        observer.complete();
      });
    }

    return this.http.get<Response>('assets/appsettings.json')
      .pipe(
        tap(data => this.settings = this.extractData(data)),
        catchError(error => this.handleErrors(error)));
  }

  getExpressions(): Observable<string[]> {

    if (this.expressions) {
      return new Observable<string[]>(observer => {
        observer.next(this.expressions);
        observer.complete();
      });
    }

    return this.http.get<Response>('assets/expressions.json')
      .pipe(
        tap(data => this.expressions = this.extractData(data)),
        catchError(error => this.handleErrors(error)));
  }

  initialize() : Promise<[AppSettingsDto, string[]]> {
    return Promise.all([this.getSettings().toPromise(), this.getExpressions().toPromise()])
  }

  private extractData(res: Response): any {
    return res || {};
  }

  private handleErrors(error: any): Observable<any> {
    console.error('An error occurred', error);
    return throwError(error.message || error);
  }
}
