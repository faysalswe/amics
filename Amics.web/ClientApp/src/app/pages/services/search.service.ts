import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { pmSearch, pmItemSearchResult } from "../models/pmsearch";
import { ItemClass, ItemCode, ItemType, Uom } from "../models/searchModels";
import { Warehouse, WarehouseLocation } from "../models/warehouse";


@Injectable({
    providedIn: "root",
})
export class SearchService {

    private readonly api = '{apiUrl}/api/search';

    constructor(private readonly httpClient: HttpClient) { } 
    
    getWarehouseInfo(warehouse: string): Observable<Warehouse[]> {
        return this.httpClient.get<Warehouse[]>(`${this.api}/Warehouse?searchWarehouse=${warehouse}`);
    }

    getLocationInfo(warehouseId: string, location: string): Observable<WarehouseLocation[]> {
        let url = `${this.api}/Location?warehouseId=${warehouseId}&searchLocation=${location}`;
        return this.httpClient.get<WarehouseLocation[]>(url);
    }
    getItemType(itemTypeId: string, itemType: string): Observable<ItemType[]> {
        let url = `${this.api}/ItemType?itemtypeId=${itemTypeId}&itemtype=${itemType}`;
        return this.httpClient.get<ItemType[]>(url);
    }
    getItemClass(itemclassId: string, itemclass: string): Observable<ItemClass[]> {
        let url = `${this.api}/ItemClass?itemclassId=${itemclassId}&itemclass=${itemclass}`;

        return this.httpClient.get<ItemClass[]>(url);
    }
    getItemCode(itemcodeId: string, itemcode: string): Observable<ItemCode[]> {
        let url = `${this.api}/ItemCode?itemcodeId=${itemcodeId}&itemcode=${itemcode}`;
        return this.httpClient.get<ItemCode[]>(url);
    }

    getUom(uomId: string, uomRef: string): Observable<Uom[]> {
        let url = `${this.api}/Uom?uomId=${uomId}&uomRef=${uomRef}`;
        return this.httpClient.get<Uom[]>(url);
    }

    getItemNumberDetails(warehouseId: string, location: string): Observable<WarehouseLocation[]> {
        let url = `${this.api}/Location/${warehouseId}`;
        if (location !== '') {
            url = url + `/${location}`
        }
        return this.httpClient.get<WarehouseLocation[]>(url);
    }

    getItemNumberSearchResults(pmSearch: pmSearch): Observable<pmItemSearchResult[]> {

        let url = `${this.api}/ItemNumber?itemNumber=${pmSearch.itemnumber}&description=${pmSearch.description}&itemType=${pmSearch.itemtype}&itemCode=${pmSearch.itemcode}&itemclass=${pmSearch.itemclass}`;
        return this.httpClient.get<pmItemSearchResult[]>(url);
    }

}