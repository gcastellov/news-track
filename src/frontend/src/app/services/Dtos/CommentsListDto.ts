import { CommentDto } from "./CommentDto";

export class CommentsListDto {
    comments: CommentDto[] = [];
    count: number = 0;
}