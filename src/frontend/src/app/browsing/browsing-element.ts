export class BrowsingElement {
    isSelected: boolean;
    content: string;
    id: string;

    constructor(isSelected: boolean, content: string) {
        this.isSelected = isSelected;
        this.content = content;
        this.id = this.getId();
    }

    getId(): string {
        return '_' + Math.random().toString(36).substr(2, 9);
    }
}
