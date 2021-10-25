import { DraftDigestDto } from './DraftDigestDto';

export class DraftSuggestionsDto {
    id: string = '';
    tags: string[] = [];
    drafts: DraftDigestDto[] = [];
}
