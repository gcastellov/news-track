export class TokenResponseDto {
    isSuccessful: boolean;
    token: string;
    username: string;
    at: Date;
    failure: number;
}
