import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { warehouse } from "../models/warehouse";


@Injectable({
    providedIn: "root",
})
export class SearchService {

    private readonly api = '{apiUrl}/api/search';

    constructor(private readonly httpClient: HttpClient) { }

    getWarehouseInfo(warehouse: string): Observable<warehouse[]> {
        return this.httpClient.get<warehouse[]>(`${this.api}/Warehouse/${warehouse}`);
    }
    getLocationInfo(warehouseId: string, location:string): Observable<Location[]> {
        let url = `${this.api}/Location/${warehouseId}`;
        if(location !== '')
        {
            url = url+`/${location}`
        }
        return this.httpClient.get<Location[]>(url);
    }
}