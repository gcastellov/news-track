export class Envelope<T> {
    isSuccessful: boolean;
    errorMessage: string;
    at: string;
    payload: T;

    constructor(payload: T) {
        this.payload = payload;
        this.isSuccessful = true;
        this.at = new Date().toUTCString();
    }

    static AsFailure<T>(payload?: T): Envelope<T> {
        const envelope = new Envelope(payload);
        envelope.isSuccessful = false;
        envelope.errorMessage = 'Seomething went wrong';
        return envelope;
    }
}
