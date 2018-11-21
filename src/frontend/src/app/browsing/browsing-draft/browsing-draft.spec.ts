import { BrowsingDraft } from './bowsing-draft';
import { BackendApiService } from '../../services/backend-api.service';
import { BrowsingElement } from './browsing-element';
import { DataBuilder } from '../../testing/data.builder';
import { IBrowseResult } from '../../services/Dtos/IBrowseResult';
import { Observable } from 'rxjs/Observable';
import { DraftResponseDto } from '../../services/Dtos/DraftResponseDto';
import { DraftRelationshipResponseDto } from '../../services/Dtos/DraftRelationshipResponseDto';
import { DraftRelationshipDto } from '../../services/Dtos/DraftRelationshipRequestDto';

describe('BrowsingDraft', () => {

    let component: BrowsingDraft;
    let data: IBrowseResult;
    const apiServiceMock = <BackendApiService> {
        setDraft: (d) => new Observable<DraftResponseDto>((observer) => observer.complete()),
        setDraftRelationship: (i, r) => new Observable<DraftRelationshipResponseDto>((observer) => observer.complete())
    };

    beforeEach(() => {
        data = {
            uri: 'http://www.domain.com/resource',
            titles: [ 'First title', 'Second title' ],
            paragraphs: ['First paragraph', 'Second paragraph', 'Third paragraph'],
            pictures: ['http://www.domain.com/imgs/img1.png', 'http://www.domain.com/imgs/img2.png']
        };

        component = new BrowsingDraft(apiServiceMock);
        component.browseResult.uri = data.uri;
        component.browseResult.titles = [
            new BrowsingElement(false, data.titles[0]),
            new BrowsingElement(true, data.titles[1])
        ];
        component.browseResult.paragraphs = [
            new BrowsingElement(true, data.paragraphs[0]),
            new BrowsingElement(true, data.paragraphs[1]),
            new BrowsingElement(false, data.paragraphs[2])
        ];
        component.browseResult.pictures = [
            new BrowsingElement(false, data.pictures[0]),
            new BrowsingElement(true, data.pictures[1])
        ];
        component.draftRequest.url = component.browseResult.uri;
        component.draftRequest.title = component.browseResult.titles
            .find((t) => t.isSelected)
            .content;
        component.draftRequest.paragraphs = component.browseResult.paragraphs
            .filter((p) => p.isSelected)
            .map((p) => p.content);
        component.draftRequest.picture = component.browseResult.pictures
            .find((p) => p.isSelected)
            .content;
        component.tags = DataBuilder.getTags();
        component.relationship = [
            new DraftRelationshipDto('some-id', 'other title', 'http://www.some-other-domain.com/resource')
        ];
    });

    it('should set data', () => {
        component.initialize();
        component.setData(data);

        expect(component.browseResult.uri).toBe(data.uri);
        expect(component.browseResult.titles.length).toEqual(data.titles.length);
        component.browseResult.titles.forEach((t) => {
            expect(t.isSelected).toBeFalsy();
            expect(t.content).toBeDefined();
            expect(data.titles.indexOf(t.content)).toBeGreaterThan(-1);
        });
        component.browseResult.paragraphs.forEach((p) => {
            expect(p.isSelected).toBeFalsy();
            expect(p.content).toBeDefined();
            expect(data.paragraphs.indexOf(p.content)).toBeGreaterThan(-1);
        });
        component.browseResult.pictures.forEach((p) => {
            expect(p.isSelected).toBeFalsy();
            expect(p.content).toBeDefined();
            expect(data.pictures.indexOf(p.content)).toBeGreaterThan(-1);
        });
    });

    it('should initialize data when is already in use', () => {
        component.initialize();

        expect(component.browseResult.uri).toBeNull();
        expect(component.browseResult.titles.length).toEqual(0);
        expect(component.browseResult.paragraphs.length).toEqual(0);
        expect(component.browseResult.pictures.length).toEqual(0);
        expect(component.relationship.length).toEqual(0);
        expect(component.tags.length).toEqual(0);
        expect(component.draftRequest.paragraphs.length).toEqual(0);
        expect(component.draftRequest.tags.length).toEqual(0);
        expect(component.draftRequest.url).toBeNull();
        expect(component.draftRequest.title).toBeNull();
        expect(component.draftRequest.picture).toBeNull();
    });

    it('should verify that is completed', () => {
        expect(component.isCompleted()).toBeTruthy();
    });

    it('should verify that is not completed because there is no title', () => {
        component.draftRequest.title = null;
        expect(component.isCompleted()).toBeFalsy();
    });

    it('should verify that is not completed because there is no paragraphs', () => {
        component.draftRequest.paragraphs = [];
        expect(component.isCompleted()).toBeFalsy();
    });

    it('should verify that is not completed because there is no picture', () => {
        component.draftRequest.picture = null;
        expect(component.isCompleted()).toBeFalsy();
    });

    it('should save the draft along with relationships and initialize the model again', () => {
        const dto = new DraftResponseDto();
        dto.id = 'response-id';
        dto.isSuccessful = true;

        const setDataMock = spyOn(apiServiceMock, 'setDraft')
            .and.returnValue(new Observable<DraftResponseDto>((o) => o.next(dto)));

        const setRelationshipMock = spyOn(apiServiceMock, 'setDraftRelationship')
            .and.returnValue(new Observable<DraftRelationshipResponseDto>((o) => o.complete));

        component.save().subscribe((o) => {
            expect(o).toBeTruthy();
            expect(setRelationshipMock).toHaveBeenCalled();
        });

        expect(component.isCompleted()).toBeFalsy();
        expect(setDataMock).toHaveBeenCalledWith(component.draftRequest);
    });
});
