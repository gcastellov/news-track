export class Tag {
    name: string;
    score: number;
    importance: number;

    constructor(name: string, score: number) {
        this.name = name;
        this.score = score;
        this.importance = 0;
    }
}
