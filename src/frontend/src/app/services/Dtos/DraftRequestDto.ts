export class DraftRequestDto {
    url: string;
    picture: string;
    title: string;
    paragraphs: string[];
    tags: string[];

    constructor() {
        this.paragraphs = [];
        this.tags = [];
    }
}
