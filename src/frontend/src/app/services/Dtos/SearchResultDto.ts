export class SearchResultDto {
    id: string = '';
    content: string = '';

    public toString = (): string => {
        return this.content;
    }
}
