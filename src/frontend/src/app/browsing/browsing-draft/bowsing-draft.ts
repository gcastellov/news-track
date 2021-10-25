import { Injectable } from '@angular/core';
import { BrowsingObject } from './browsing-object';
import { DraftRequestDto } from '../../services/Dtos/DraftRequestDto';
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';
import { BackendApiService } from '../../services/backend-api.service';
import { IBrowseResult } from '../../services/Dtos/IBrowseResult';
import { BrowsingElement } from './browsing-element';
import { Observable } from 'rxjs';

@Injectable()
export class BrowsingDraft {
    url: string;
    tags: string[];
    browseResult: BrowsingObject;
    draftRequest: DraftRequestDto;
    relationship: DraftRelationshipDto[];

    constructor(private _apiService: BackendApiService) {
        this.tags = [];
        this.url = '';
        this.relationship = [];
        this.browseResult = new BrowsingObject();
        this.draftRequest = new DraftRequestDto();
    }

    initialize() {
        this.tags = [];
        this.relationship = [];
        this.browseResult = new BrowsingObject();
        this.draftRequest = new DraftRequestDto();
    }

    isCompleted(): boolean {
        return this.draftRequest.picture !== '' &&
            this.draftRequest.title !== '' &&
            this.draftRequest.paragraphs.length > 0;
    }

    save(): Observable<boolean>  {
        return new Observable(observer => {
            this._apiService.setDraft(this.draftRequest).subscribe(r => {
                if (r.isSuccessful) {
                    this.saveRelationship(r.payload.id);
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
