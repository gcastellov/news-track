import { Injectable } from '@angular/core';
import { BrowsingObject } from './browsing-object';
import { DraftRequestDto } from '../../services/Dtos/DraftRequestDto';
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';
import { BackendApiService } from '../../services/backend-api.service';
import { Observable } from 'rxjs/Observable';
import { IBrowseResult } from '../../services/Dtos/IBrowseResult';
import { BrowsingElement } from './browsing-element';

@Injectable()
export class BrowsingDraft {
    url: string;
    tags: string[];
    browseResult: BrowsingObject;
    draftRequest: DraftRequestDto;
    relationship: DraftRelationshipDto[];

    constructor(private _apiService: BackendApiService) {
        this.tags = [];
        this.relationship = [];
        this.browseResult = new BrowsingObject();
        this.draftRequest = new DraftRequestDto();
    }

    initialize() {
        this.url = null;
        this.tags = [];
        this.relationship = [];
        this.browseResult.paragraphs = [];
        this.browseResult.pictures = [];
        this.browseResult.titles = [];
        this.browseResult.uri = null;
        this.draftRequest.paragraphs = [];
        this.draftRequest.picture = null;
        this.draftRequest.tags = [];
        this.draftRequest.title = null;
        this.draftRequest.url = null;
    }

    isCompleted(): boolean {
        return this.draftRequest &&
            this.draftRequest.picture &&
            this.draftRequest.title &&
            this.draftRequest.paragraphs &&
            this.draftRequest.paragraphs.length > 0;
    }

    save(): Observable<boolean>  {
        return new Observable(observer => {
            this._apiService.setDraft(this.draftRequest).subscribe(r => {
                if (r.isSuccessful) {
                    this.saveRelationship(r.id);
                    this.initialize();
                    observer.next(true);
                }}
            );
        });
    }

    setData(data: IBrowseResult) {
        this.browseResult.uri = data.uri;
        this.browseResult.titles = data.titles.map(t => new BrowsingElement(false, t));
        this.browseResult.paragraphs = data.paragraphs.map(p => new BrowsingElement(false, p));
        this.browseResult.pictures = data.pictures.map(p => new BrowsingElement(false, p));
        this.draftRequest.url = this.browseResult.uri;
    }

    private saveRelationship(id: string) {
        if (id && this.relationship && this.relationship.length > 0) {
          this._apiService.setDraftRelationship(id, this.relationship).subscribe();
        }
    }
}
