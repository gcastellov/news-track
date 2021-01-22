import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { AppSettingsDto } from './Dtos/AppSettingsDto';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AppSettingsService {

  settings: AppSettingsDto;
  expressions: string[];

  constructor(private http: HttpClient) {
  }

  getSettings(): Observable<AppSettingsDto> {
    if (this.settings) {
      return new Observable<AppSettingsDto>(observer => observer.next(this.settings));
    }

    return this.http.get<Response>('assets/appsettings.json')
      .map(data => this.settings = this.extractData(data))
      .catch(this.handleErrors);
  }

  getExpressions(): Observable<string[]> {
    if (this.expressions) {
      return new Observable<string[]>(observer => observer.next(this.expressions));
    }

    return this.http.get<Response>('assets/expressions.json')
      .map(data => this.expressions = this.extractData(data))
      .catch(this.handleErrors);
  }

  private extractData(res: Response): any {
    return res || {};
  }

  private handleErrors(error: any): Observable<any> {
    console.error('An error occurred', error);
    return Observable.throw(error.message || error);
  }

  initialize() {
    return Promise.all([this.getSettings().toPromise(), this.getExpressions().toPromise()])
      .then(() => {
        console.log('settings intialized');
      })
      .catch(err => console.log(err));
  }
}
