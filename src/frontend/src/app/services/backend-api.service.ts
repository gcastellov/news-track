import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
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
import { ChangePassworRequestDto } from './Dtos/ChangePasswordRequestDto';
import { DraftSuggestionIdsDto } from './Dtos/DraftSuggestionIdsDto';
import { CreateIdentityRequestDto } from './Dtos/CreateIdentityRequestDto';
import { Envelope, UntypedEnvelope } from './Dtos/Envelope';
import { Observable } from 'rxjs';
import { CommentDto } from './Dtos/CommentDto';
import { CreateCommentDto } from './Dtos/CreateCommentDto';
import { CommentsListDto } from './Dtos/CommentsListDto';

@Injectable()
export class BackendApiService {

  constructor(private _client: HttpClient, private _authService: AuthenticationApiService) {
  }

  browse(url: string): Observable<Envelope<IBrowseResult>> {
    const browseUrl = `${environment.baseUrl}/api/browser/browse`;
    const params = new HttpParams().set('url', url);
    const headers = this._authService.getTokenHeaders();
    return this._client.get<Envelope<IBrowseResult>>(browseUrl, {params: params, headers: headers});
  }

  setDraft(request: DraftRequestDto): Observable<Envelope<DraftResponseDto>> {
    const draftUrl = `${environment.baseUrl}/api/draft`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<Envelope<DraftResponseDto>>(draftUrl, request, {headers: headers});
  }

  setDraftRelationship(id: string, relationship: DraftRelationshipDto[]): Observable<Envelope<DraftRelationshipResponseDto>> {
    const relationshipUrl = `${environment.baseUrl}/api/draft/${id}/relationship`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<Envelope<DraftRelationshipResponseDto>>(relationshipUrl, relationship, {headers: headers});
  }

  setVisit(id: string): Observable<Envelope<IncrementalResponseDto>> {
    const setVisitUrl = `${environment.baseUrl}/api/news/entry/${id}/visit`;
    return this._client.patch<Envelope<IncrementalResponseDto>>(setVisitUrl, null);
  }

  setFuck(id: string): Observable<Envelope<IncrementalResponseDto>> {
    const setFuckUrl = `${environment.baseUrl}/api/news/entry/${id}/fuck`;
    return this._client.patch<Envelope<IncrementalResponseDto>>(setFuckUrl, null);
  }

  getLatestDrafts(page: number, take: number): Observable<Envelope<DraftListDto>> {
    const newsUrl = `${environment.baseUrl}/api/news/latest`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<Envelope<DraftListDto>>(newsUrl, {params: params});
  }

  getMostViewedDrafts(page: number, take: number): Observable<Envelope<DraftListDto>> {
    const newsUrl = `${environment.baseUrl}/api/news/mostviewed`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<Envelope<DraftListDto>>(newsUrl, {params: params});
  }

  getMostFuckedDrafts(page: number, take: number): Observable<Envelope<DraftListDto>> {
    const newsUrl = `${environment.baseUrl}/api/news/mostfucked`;
    const params = new HttpParams()
      .set('page', page.toString())
      .set('count', take.toString());
    return this._client.get<Envelope<DraftListDto>>(newsUrl, {params: params});
  }

  getLatest(take: number): Observable<Envelope<DraftDigestDto[]>> {
    const latestUrl = `${environment.baseUrl}/api/news/top/latest`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<Envelope<DraftDigestDto[]>>(latestUrl, {params: params});
  }

  getMostViewed(take: number): Observable<Envelope<DraftDigestDto[]>> {
    const mostViewedUrl = `${environment.baseUrl}/api/news/top/viewed`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<Envelope<DraftDigestDto[]>>(mostViewedUrl, {params: params});
  }

  getMostFucking(take: number): Observable<Envelope<DraftDigestDto[]>> {
    const mostFuckingUrl = `${environment.baseUrl}/api/news/top/fucking`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<Envelope<DraftDigestDto[]>>(mostFuckingUrl, {params: params});
  }

  getDraft(id: string): Observable<Envelope<DraftDto>> {
    const newsUrl = `${environment.baseUrl}/api/news/entry/${id}`;
    return this._client.get<Envelope<DraftDto>>(newsUrl);
  }

  getDraftRelationship(id: string): Observable<Envelope<DraftDigestDto[]>> {
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/relationship`;
    return this._client.get<Envelope<DraftDigestDto[]>>(relationshipUrl);
  }

  getDraftSuggestions(id: string, take: number): Observable<Envelope<DraftSuggestionsDto>> {
    const params = new HttpParams().set('take', take.toString());
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/suggestions`;
    return this._client.get<Envelope<DraftSuggestionsDto>>(relationshipUrl, {params: params});
  }

  getAllDraftSuggestions(id: string, take: number, skip: number): Observable<Envelope<DraftSuggestionIdsDto>> {
    const params = new HttpParams()
      .set('take', take.toString())
      .set('skip', skip.toString());
    const relationshipUrl = `${environment.baseUrl}/api/news/entry/${id}/suggestions/all`;
    return this._client.get<Envelope<DraftSuggestionIdsDto>>(relationshipUrl, {params: params});
  }

  getTags(): Observable<Envelope<string[]>> {
    const tagsUrl = `${environment.baseUrl}/api/tags`;
    return this._client.get<Envelope<string[]>>(tagsUrl);
  }

  getWebsites(take: number): Observable<Envelope<WebsiteStatsDto[]>> {
    const websitesUrl = `${environment.baseUrl}/api/news/top/websites`;
    const params = new HttpParams().set('take', take.toString());
    return this._client.get<Envelope<WebsiteStatsDto[]>>(websitesUrl, {params: params});
  }

  getStatsTags(): Observable<Envelope<TagsStatsResponseDto>> {
    const tagsUrl = `${environment.baseUrl}/api/tags/stats`;
    return this._client.get<Envelope<TagsStatsResponseDto>>(tagsUrl);
  }

  search(pattern: string): Observable<Envelope<SearchResultDto[]>> {
    const searchUrl = `${environment.baseUrl}/api/search`;
    const params = new HttpParams().set('query', pattern);
    return this._client.get<Envelope<SearchResultDto[]>>(searchUrl, {params: params});
  }

  advancedSearch(website: string, pattern: string, tags: string[], page: number, take: number): Observable<Envelope<DraftListDto>> {
    const newsUrl = `${environment.baseUrl}/api/search/advanced`;
    let params = new HttpParams()
      .set('website', website)
      .set('query', pattern)
      .set('page', page.toString())
      .set('count', take.toString());

    for (let i = 0; i < tags.length; i++) {
        params = params.append('tags', tags[i]);
    }

    return this._client.get<Envelope<DraftListDto>>(newsUrl, {params: params});
  }

  checkWebsite(url: string): Observable<Envelope<WebsiteDto>> {
    const checkUrl = `${environment.baseUrl}/api/website/check`;
    const params = new HttpParams().set('uri', url);
    const headers = this._authService.getTokenHeaders();
    return this._client.get<Envelope<WebsiteDto>>(checkUrl, {params: params, headers: headers});
  }

  getIdentity(): Observable<Envelope<IdentityDto>> {
    const url = `${environment.baseUrl}/api/identity`;
    const headers = this._authService.getTokenHeaders();
    return this._client.get<Envelope<IdentityDto>>(url, {headers: headers});
  }

  changePassword(req: ChangePassworRequestDto): Observable<UntypedEnvelope> {
    const url = `${environment.baseUrl}/api/identity/password/change`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<UntypedEnvelope>(url, req, { headers: headers });
  }

  processSuggestions(): Observable<boolean> {
    const url = `${environment.baseUrl}/api/content/suggestions`;
    const headers = this._authService.getTokenHeaders();
    const observable = this._client.post<any>(url, null, { headers: headers });

    return new Observable<boolean>(observer => {
      observable.subscribe((res: any) => {
        observer.next(res.statusCode === 202);
        observer.complete();
      });
    });
  }

  createUser(req: CreateIdentityRequestDto): Observable<UntypedEnvelope> {
    const url = `${environment.baseUrl}/api/identity/create`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<UntypedEnvelope>(url, req, { headers: headers });
  }

  signUp(req: CreateIdentityRequestDto): Observable<UntypedEnvelope> {
    const url = `${environment.baseUrl}/api/identity/signup`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<UntypedEnvelope>(url, req, { headers: headers });
  }

  comment(req: CreateCommentDto): Observable<Envelope<CommentDto>> {
    const url = `${environment.baseUrl}/api/comment`;
    const headers = this._authService.getTokenHeaders();
    return this._client.post<Envelope<CommentDto>>(url, req,  { headers: headers });
  }

  getCommentsByDraftId(draftId: string, take: number, skip: number): Observable<Envelope<CommentsListDto>> {
    const params = new HttpParams()
      .set('count', take.toString())
      .set('page', skip.toString());      
    const url = `${environment.baseUrl}/api/comment/news/${draftId}`;
    return this._client.get<Envelope<CommentsListDto>>(url, { params: params });
  }

  getCommentsByCommentId(commentId: string, take: number, skip: number): Observable<Envelope<CommentsListDto>> {
    const params = new HttpParams()
      .set('count', take.toString())
      .set('page', skip.toString());      
    const url = `${environment.baseUrl}/api/comment/${commentId}/replies`;
    return this._client.get<Envelope<CommentsListDto>>(url, { params: params });
  }

  getComment(commentId: string): Observable<Envelope<CommentDto>> {
    const url = `${environment.baseUrl}/api/comment/${commentId}`;
    return this._client.get<Envelope<CommentDto>>(url);
  }

  setLike(commentId: string): Observable<Envelope<IncrementalResponseDto>> {
    const url = `${environment.baseUrl}/api/comment/${commentId}/like`;
    const headers = this._authService.getTokenHeaders();
    return this._client.patch<Envelope<IncrementalResponseDto>>(url, null, { headers: headers });
  }

}
