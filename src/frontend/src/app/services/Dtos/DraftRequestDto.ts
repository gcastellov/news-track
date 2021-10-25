export class DraftRequestDto {
    url: string;
    picture: string;
    title: string;
    paragraphs: string[];
    tags: string[];

    constructor() {
        this.url = '';
        this.picture = '';
        this.title = '';
        this.paragraphs = [];
        this.tags = [];
    }
}
