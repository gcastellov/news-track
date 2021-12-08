export class CreateCommentDto {    
    draftId: string;
    content: string;    
    replyingTo: string | undefined;

    constructor(draftId: string, content: string) {
        this.draftId = draftId;
        this.content = content;
    }
}