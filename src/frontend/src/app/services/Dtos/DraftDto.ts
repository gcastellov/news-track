export class DraftDto {
    id: string;
    uri: string;
    picture: string;
    title: string;
    paragraphs: string[];
    tags: string[];
    createdAt: Date;
    views: number;
    fucks: number;
    related: number;
    createdBy: string;
}
