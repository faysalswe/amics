import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Warehouse, WarehouseLocation } from "../models/warehouse";


@Injectable({
    providedIn: "root",
})
export class SearchService {

    private readonly api = '{apiUrl}/api/search';

    constructor(private readonly httpClient: HttpClient) { }

    getWarehouseInfo(warehouse: string): Observable<Warehouse[]> {
        return this.httpClient.get<Warehouse[]>(`${this.api}/Warehouse/${warehouse}`);
    }
    getLocationInfo(warehouseId: string, location:string): Observable<WarehouseLocation[]> {
        let url = `${this.api}/Location/${warehouseId}`;
        if(location !== '')
        {
            url = url+`/${location}`
        }
        return this.httpClient.get<WarehouseLocation[]>(url);
    }
}