export class DraftDto {
    id: string = '';
    uri: string = '';
    picture: string = '';
    title: string = '';
    paragraphs: string[] = [];
    tags: string[] = [];
    createdAt: Date = new Date();
    views: number = 0;
    fucks: number = 0;
    related: number = 0;
    createdBy: string = '';
}
