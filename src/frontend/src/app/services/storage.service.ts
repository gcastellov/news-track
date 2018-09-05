export class StorageService {

    setItem(key: string, value: string) {
        sessionStorage.setItem(key, value);
    }

    getItem(key: string): string {
        return sessionStorage.getItem(key);
    }

}
