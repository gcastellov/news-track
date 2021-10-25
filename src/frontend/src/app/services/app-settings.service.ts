import { Injectable } from '@angular/core';
import { AppSettingsDto } from './Dtos/AppSettingsDto';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

@Injectable()
export class AppSettingsService {

  settings: AppSettingsDto;
  expressions: string[] | undefined;

  constructor(private http: HttpClient) {
    this.settings = new AppSettingsDto();
  }

  getSettings(): Observable<AppSettingsDto> {
    return this.http.get<Response>('assets/appsettings.json')
      .pipe(
        tap(data => this.settings = this.extractData(data)),
        catchError(error => this.handleErrors(error)));
  }

  getExpressions(): Observable<string[]> {
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
