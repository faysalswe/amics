import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { InventoryStatus } from "../components/InventoryStatus/inventory.status.component";

@Injectable({
    providedIn: "root",
})
export class InventoryService {

    private readonly api = '{apiUrl}/api/Inventory';

    constructor(private readonly httpClient: HttpClient) { }

    getInventoryStatus(val: string, secUserId: String) {
        return this.httpClient.get<InventoryStatus>(`${this.api}?ItemsId=${val}&SecUserId=${secUserId}`);
    }

}