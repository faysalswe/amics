import { registerLocaleData } from "@angular/common";
import { HttpClient} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { BulkTransferItem,BulkTransferItemResult,UpdateBulkTransfer,LstMessage } from "../models/bulktransfer";


@Injectable({
    providedIn: "root",
})

export class BulkTransferService {
    private readonly api = '{apiUrl}/api/BulkTransfer';

    constructor(private readonly httpClient: HttpClient) { }    
    
        //validate from Warehouse and To Location, gets location id
        validateBulkTransferItem(warehouse: string, location: string) {        
        let url = `${this.api}/ValidateLocation?warehouse=${warehouse }&location=${location}`;
        return this.httpClient.get<string[]>(url);        
        }
        
        //Gets Item details
        getBulkTransferItemDetails(warehouse: string, location: string) {         
            let url = `${this.api}/BulkTransferView?warehouse=${warehouse }&location=${location}`;
            return this.httpClient.get<BulkTransferItemResult[]>(url);
        }

        //Execute Bulk Transfer Items
         executeBulkTransferItemDetails(req: UpdateBulkTransfer) {   
            let url = `${this.api}/ExecuteBulkTransfer`;                         
            return this.httpClient.post<LstMessage[]>(`${this.api}/ExecuteBulkTransfer`, req);
        }
}