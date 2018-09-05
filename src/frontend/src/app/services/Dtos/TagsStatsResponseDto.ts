export class TagsStatsResponseDto {
    tagsScore: TagScore[];
    maxScore: number;
    averageScore: number;
    count: number;
}

export class TagScore {
    tag: string;
    score: number;
}
