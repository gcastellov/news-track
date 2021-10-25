import { Injectable } from "@angular/core";
@Injectable()
export class StorageService {

    setItem(key: string, value: string) {
        localStorage.setItem(key, value);
    }

    getItem(key: string): string {
        return localStorage.getItem(key) ?? '';
    }

}
