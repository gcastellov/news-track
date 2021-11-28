import { ErrorDto } from "./ErrorDto";

export class UntypedEnvelope {
    isSuccessful: boolean = false;
    error: ErrorDto | undefined;
    at: string;

    constructor() {
        this.isSuccessful = true;
        this.at = new Date().toUTCString();
        this.error = undefined;
    }

    static AsFailure(errorCode: number): UntypedEnvelope {
        const envelope = new UntypedEnvelope();
        envelope.isSuccessful = false;
        envelope.error = new ErrorDto();
        envelope.error.message = 'Seomething went wrong'; 
        envelope.error.code = errorCode;
        return envelope;
    }
}

export class Envelope<T> extends UntypedEnvelope {
    payload: T;

    constructor(payload: T) {
        super();
        this.payload = payload;
    }
}
