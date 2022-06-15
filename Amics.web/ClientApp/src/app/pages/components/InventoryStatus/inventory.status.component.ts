import { Component, Input } from "@angular/core";
import { IncreaseInventoryService } from "../../services/increase.inventory.service";
import { HttpClient } from "@angular/common/http";
import { InventoryService } from "../../services/inventory.service";


export interface InventoryStatus {
    pn: string
    descr: string
    allocated: number
    avail: number
    notavail: number
    total: number
}

@Component({
    selector: "app-inventory-status",
    templateUrl: 'inventory.status.component.html',
    styleUrls: ['./inventory.status.component.scss'],
    providers: [IncreaseInventoryService]
})
export class InventoryStatusComponent {

    @Input()
    private _itemsId: string = '';

    @Input()
    secUserId: string = '';
    
    onHand: number = 0;
    notAvailable: number = 0;
    allocated: number = 0;
    available: number = 0;

    constructor(private inventoryService: InventoryService) {
    }

    @Input()
    public set itemsId(val: string) {
        this._itemsId = val;
        this.inventoryService.getInventoryStatus(this._itemsId, this.secUserId).subscribe((obj: InventoryStatus) => {
            this.onHand = obj.total;
            this.notAvailable = obj.notavail;
            this.allocated = obj.allocated;
            this.available = obj.avail;
            console.log(obj);
        });
    }

}
