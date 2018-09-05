import { DraftDto } from '../services/Dtos/DraftDto';
import { AppSettingsDto } from '../services/Dtos/AppSettingsDto';
import { DraftDigestDto } from '../services/Dtos/DraftDigestDto';
import { DraftSuggestionsDto } from '../services/Dtos/DraftSuggestionsDto';
import { DraftListDto } from '../services/Dtos/DraftListDto';
import { TagsStatsResponseDto, TagScore } from '../services/Dtos/TagsStatsResponseDto';
import { WebsiteStatsDto } from '../services/Dtos/WebsiteStatsDto';
import { IdentityDto } from '../services/Dtos/IdentityDto';
import { DraftSuggestionIdsDto } from '../services/Dtos/DraftSuggestionIdsDto';

export class DataBuilder {
    constructor() {
    }

    static getWebsitesStatsDto(): WebsiteStatsDto[] {
        return this.getWebsites().map(w => {
            const website = new WebsiteStatsDto();
            website.count = 10;
            website.name = w;
            return website;
        });
    }

    static getTagsStatsDto(): TagsStatsResponseDto {
        const stats = new TagsStatsResponseDto();
        stats.tagsScore = this.getTags().map(t => {
            const score = new TagScore();
            score.tag = t;
            score.score = 20;
            return score;
        });

        stats.count = stats.tagsScore.length;
        stats.maxScore = 20;
        stats.averageScore = 20;
        return stats;
    }

    static getDraftListDto(): DraftListDto {
        const list = new DraftListDto();
        list.news = this.getDraftsDto();
        list.count = list.news.length;
        return list;
    }

    static getDraftSuggestionDto(): DraftSuggestionsDto {
        const drafts = this.getDraftsDto();
        const suggestion = new DraftSuggestionsDto();
        suggestion.id = drafts[0].id;
        suggestion.tags = this.getTags();
        suggestion.drafts = this.getDraftDigestsDto(drafts.splice(1, drafts.length - 1));
        return suggestion;
    }

    static getDraftSuggesionIdsDto(id: string): DraftSuggestionIdsDto {
        const suggestion = new DraftSuggestionIdsDto();
        suggestion.id = id;
        suggestion.suggestedIds = this.getDraftSuggestionDto().drafts.map(d => d.id);
        suggestion.count = suggestion.suggestedIds.length;
        return suggestion;
    }

    static getDraftDigestsDto(drafts?: DraftDto[]): DraftDigestDto[] {
        if (!drafts) {
            drafts = this.getDraftsDto();
        }

        return drafts.map(d => {
            const digest = new DraftDigestDto();
            digest.id = d.id;
            digest.title = d.title;
            digest.url = d.uri;
            digest.views = d.views;
            return digest;
        });
    }

    static getDraftsDto(): DraftDto[] {
        return [
            this.createDraftDto(45, 33, 'http://www.somedomain.com/resource-one', 'id-one'),
            this.createDraftDto(67, 23, 'http://www.somedomain.com/resource-two', 'id-two'),
            this.createDraftDto(88, 79, 'http://www.somedomain.com/resource-three', 'id-three')
        ];
    }

    static getExpressions(): string[] {
        return [
            'Wowww',
            'This rocks man!',
            'Wtf!',
            'Oooh la la'
        ];
    }

    static getSettings(): AppSettingsDto {
        const settings = new AppSettingsDto();
        settings.corporation = 'The corporation';
        settings.country = 'Spain';
        settings.brand = 'NewsTrack';
        settings.corporation = 'NewsTrack & Co';
        settings.defaultLanguage = 'en';
        settings.githubUrl = 'http://www.someurl.com';
        return settings;
    }

    static getTags(): string[] {
        return ['One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven'];
    }

    static getWebsites(): string[] {
        return [
            'http://one.domain.com',
            'http://two.domain.com',
            'http://three.domain.com'
        ];
    }

    static getIdentityDto(): IdentityDto {
        const identity = new IdentityDto();
        identity.accessFailedCount = 1;
        identity.createdAt = new Date().toUTCString();
        identity.email = 'subject@domain.com';
        identity.idType = 1;
        identity.isEnabled = true;
        identity.username = 'myUsername';
        return identity;
    }

    private static createDraftDto(fucks: number, views: number, uri: string, id: string): DraftDto {
        const draftDto = new DraftDto();
        draftDto.createdAt = new Date();
        draftDto.createdBy = 'UserOne';
        draftDto.fucks = fucks;
        draftDto.id = id;
        draftDto.uri = uri;
        draftDto.views = views;
        draftDto.title = `My ${draftDto.id} title`;
        draftDto.paragraphs = [ 'This is the first paragraph',  'This is the second paragraph' ];
        return draftDto;
    }
}
