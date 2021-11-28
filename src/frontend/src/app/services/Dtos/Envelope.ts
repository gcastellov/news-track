import { ErrorDto } from "./ErrorDto";

export class Envelope<T> {
    isSuccessful: boolean = false;
    error: ErrorDto | undefined;
    at: string;
    payload: T;

    constructor(payload: T) {
        this.payload = payload;
        this.isSuccessful = true;
        this.at = new Date().toUTCString();
        this.error = undefined;
    }

    static AsFailure<T>(payload: T): Envelope<T> {
        const envelope = new Envelope(payload);
        envelope.isSuccessful = false;
        envelope.error = new ErrorDto();
        envelope.error.message = 'Seomething went wrong'; 
        envelope.error.code = 0;
        return envelope;
    }
}
