import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {
    DefaultValInt, ERInt
} from "../../../shared/models/rest.api.interface.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn: "root",
})
export class IncreaseInventoryService {

    private readonly apiInventory = '{apiUrl}/api/Inventory';
    private readonly apiSearch = '{apiUrl}/api/Search';

    constructor(private readonly httpClient: HttpClient) {
    }

    getDefaultValues(): Observable<DefaultValInt[]> {
        let url = `${this.apiInventory}/getDefaultValues`;
        return this.httpClient.get<DefaultValInt[]>(url);
    }



    getER(id: string): Observable<ERInt[]> {
        var param = {
            ItemsId: id
        };
        let url = `${this.apiInventory}/getErLookup`;
        return this.httpClient.get<ERInt[]>(url, { params: param });
    }


}
