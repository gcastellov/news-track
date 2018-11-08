import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { IBrowseResult } from './Dtos/IBrowseResult';
import { DraftRequestDto } from './Dtos/DraftRequestDto';
import { DraftResponseDto } from './Dtos/DraftResponseDto';
import { DraftDto } from './Dtos/DraftDto';
import { DraftListDto } from './Dtos/DraftListDto';
import { DraftDigestDto } from './Dtos/DraftDigestDto';
import { SearchResultDto } from './Dtos/SearchResultDto';
import { DraftRelationshipDto } from './Dtos/DraftRelationshipRequestDto';
import { DraftRelationshipResponseDto } from './Dtos/DraftRelationshipResponseDto';
import { AuthenticationApiService } from './authentication-api.service';
import { TagsStatsResponseDto } from './Dtos/TagsStatsResponseDto';
import { DraftSuggestionsDto } from './Dtos/DraftSuggestionsDto';
import { WebsiteStatsDto } from './Dtos/WebsiteStatsDto';
import { IncrementalResponseDto } from './Dtos/IncrementalResponseDto';
import { WebsiteDto } from './Dtos/WebsiteDto';
import { IdentityDto } from './Dtos/IdentityDto';
import { ChangePasswordResponseDto } from './Dtos/ChangePasswordResponseDto';
import { ChangePassworRequestDto } from './Dtos/ChangePasswordRequestDto';
import { DraftSuggestionIdsDto } from './Dtos/DraftSuggestionIdsDto';
import { CreateIdentityResponseDto } from './Dtos/CreateIdentityResponseDto';
import { CreateIdentityRequestDto } from './Dtos/CreateIdentityRequestDto';

@Injectable()
export class BackendApiService {

  constructor(private _client: HttpClient, private _authService: AuthenticationApiService) {
  }

  browse(url: string): Observable<IBrowseResult> {
    const browseUrl = `${environment.baseUrl}/api/browser/browse`;
    const params = new HttpParams().set('url', url);
    const headers = this._authService.getTokenHeaders();
    return this._client.get<IBrowseResult>(browseUrl, {params: params, headers: headers});
  }

  setDraft(request: DraftRequestDto): Observable<DraftResponseDto> {
    const draftUrl = `${environment.baseUrl}/api/draft`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<DraftResponseDto>(draftUrl, request, {headers: headers});
  }

  setDraftRelationship(id: string, relationship: DraftRelationshipDto[]): Observable<DraftRelationshipResponseDto> {
    const relationshipUrl = `${environment.baseUrl}/api/draft/${id}/relationship`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<DraftRelationshipResponseDto>(relationshipUrl, relationship, {headers: headers});
  }

  setVisit(id: string): Observable<IncrementalResponseDto> {
    const setVisitUrl = `${environment.baseUrl}/api/news/entry/${id}/visit`;
    return this._client.patch<IncrementalResponseDto>(setVisitUrl, null);
  }

  setFuck(id: string): Observable<IncrementalResponseDto> {
    const setFuckUrl = `${environment.baseUrl}/api/news/entry/${id}/fuck`;
    return this._client.patch<IncrementalResponseDto>(setFuckUrl, null);
  }

  getLatestDrafts(page: number, take: number): Observable<DraftListDto> {
    const newsUrl = `${environment.baseUrl}/api/news/latest`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<DraftListDto>(newsUrl, {params: params});
  }

  getMostViewedDrafts(page: number, take: number): Observable<DraftListDto> {
    const newsUrl = `${environment.baseUrl}/api/news/mostviewed`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<DraftListDto>(newsUrl, {params: params});
  }

  getMostFuckedDrafts(page: number, take: number): Observable<DraftListDto> {
    const newsUrl = `${environment.baseUrl}/api/news/mostfucked`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<DraftListDto>(newsUrl, {params: params});
  }

  getLatest(take: number): Observable<DraftDigestDto[]> {
    const latestUrl = `${environment.baseUrl}/api/news/top/latest`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<DraftDigestDto[]>(latestUrl, {params: params});
  }

  getMostViewed(take: number): Observable<DraftDigestDto[]> {
    const mostViewedUrl = `${environment.baseUrl}/api/news/top/viewed`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<DraftDigestDto[]>(mostViewedUrl, {params: params});
  }

  getMostFucking(take: number): Observable<DraftDigestDto[]> {
    const mostFuckingUrl = `${environment.baseUrl}/api/news/top/fucking`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<DraftDigestDto[]>(mostFuckingUrl, {params: params});
  }

  getDraft(id: string): Observable<DraftDto> {
    const newsUrl = `${environment.baseUrl}/api/news/entry/${id}`;
    return this._client.get<DraftDto>(newsUrl);
  }

  getDraftRelationship(id: string): Observable<DraftDigestDto[]> {
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/relationship`;
    return this._client.get<DraftDigestDto[]>(relationshipUrl);
  }

  getDraftSuggestions(id: string, take: number): Observable<DraftSuggestionsDto> {
    const params = new HttpParams().set('take', take.toString());
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/suggestions`;
    return this._client.get<DraftSuggestionsDto>(relationshipUrl, {params: params});
  }

  getAllDraftSuggestions(id: string, take: number, skip: number): Observable<DraftSuggestionIdsDto> {
    const params = new HttpParams()
      .set('take', take.toString())
      .set('skip', skip.toString());
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/suggestions/all`;
    return this._client.get<DraftSuggestionIdsDto>(relationshipUrl, {params: params});
  }

  getTags(): Observable<string[]> {
    const tagsUrl = `${environment.baseUrl}/api/tags`;
    return this._client.get<string[]>(tagsUrl);
  }

  getWebsites(take: number): Observable<WebsiteStatsDto[]> {
    const websitesUrl = `${environment.baseUrl}/api/news/top/websites`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<WebsiteStatsDto[]>(websitesUrl, {params: params});
  }

  getStatsTags(): Observable<TagsStatsResponseDto> {
    const tagsUrl = `${environment.baseUrl}/api/tags/stats`;
    return this._client.get<TagsStatsResponseDto>(tagsUrl);
  }

  search(pattern: string): Observable<SearchResultDto[]> {
    const searchUrl = `${environment.baseUrl}/api/search`;
    const params = new HttpParams().set('query', pattern);
    return this._client.get<SearchResultDto[]>(searchUrl, {params: params});
  }

  advancedSearch(website: string, pattern: string, tags: string[], page: number, take: number): Observable<DraftListDto> {
    const newsUrl = `${environment.baseUrl}/api/search/advanced`;
    let params = new HttpParams()
      .set('website', website)
      .set('query', pattern)
      .set('page', page.toString())
      .set('count', take.toString());

    for (let i = 0; i < tags.length; i++) {
        params = params.append('tags', tags[i]);
    }

    return this._client.get<DraftListDto>(newsUrl, {params: params});
  }

  checkWebsite(url: string): Observable<WebsiteDto> {
    const checkUrl = `${environment.baseUrl}/api/website/check`;
    const params = new HttpParams().set('uri', url);
    const headers = this._authService.getTokenHeaders();
    return this._client.get<WebsiteDto>(checkUrl, {params: params, headers: headers});
  }

  getIdentity(): Observable<IdentityDto> {
    const url = `${environment.baseUrl}/api/identity`;
    const headers = this._authService.getTokenHeaders();
    return this._client.get<IdentityDto>(url, {headers: headers});
  }

  changePassword(req: ChangePassworRequestDto): Observable<ChangePasswordResponseDto> {
    const url = `${environment.baseUrl}/api/identity/password/change`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<ChangePasswordResponseDto>(url, req, { headers: headers });
  }

  processSuggestions(): Observable<boolean> {
    const url = `${environment.baseUrl}/api/content/suggestions`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post(url, null, { headers: headers }).map((res: any) => res.statusCode === 202);
  }

  createUser(req: CreateIdentityRequestDto): Observable<CreateIdentityResponseDto> {
    const url = `${environment.baseUrl}/api/identity/create`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<CreateIdentityResponseDto>(url, req, { headers: headers });
  }

}
