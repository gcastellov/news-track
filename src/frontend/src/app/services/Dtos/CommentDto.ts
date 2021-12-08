export class CommentDto {
    id: string = '';
    draftId: string = '';
    replyingTo: string = '';
    createdBy: string = '';
    createdAt: Date = new Date();
    content: string = '';
    likes: number = 0;
    replies: number = 0;
}