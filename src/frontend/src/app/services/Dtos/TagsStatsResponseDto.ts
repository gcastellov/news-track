export class TagsStatsResponseDto {
    tagsScore: TagScore[] = [];
    maxScore: number = 0;
    averageScore: number = 0;
    count: number = 0;
}

export class TagScore {
    tag: string = '';
    score: number = 0;
}
