import { Component, OnInit } from "@angular/core";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { pmBomDetails } from "src/app/pages/models/pmBomDetails";
import { PmChildType } from "src/app/pages/models/pmChildType";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { pmPoDetails } from "src/app/pages/models/pmPoDetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";

@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {
    warehouses: Warehouse[] = [];
    warehouseNames: string[] = [];
    locations: WarehouseLocation[] = [];
    validLocations: WarehouseLocation[] = [];
    validLocationNames: string[] = [];
    invType: string = '';
    bomDetails: pmBomDetails[] = [];
    poDetails: pmPoDetails[] = [];
    selectedChild: PmChildType = PmChildType.BOM;
    childType: typeof PmChildType;
    constructor(private searchService: SearchService, private pmdataTransfer: PartMasterDataTransService, private pmService: PartMasterService) { this.childType = PmChildType }

    ngOnInit(): void {

        this.searchService.getWarehouseInfo('').subscribe(w => {
            this.warehouses = w;
            this.warehouseNames = w.map(w => w.warehouse);
        })

        this.searchService.getLocationInfo('', '').subscribe(l => {
            this.locations = l;
        })

        this.pmdataTransfer.selectedItemForPMDetails$.subscribe(item => {
            console.log(item);
            this.pmDetails = item;
            this.invType = item.invType;
            this.updateWarehouseSelection(item.location);
        });
        this.pmdataTransfer.selectedItemBomForPMDetails$.subscribe(boms => {
            this.bomDetails = boms;
        })

        this.pmdataTransfer.selectedItemPoForPMDetails$.subscribe(poLines => {
            this.poDetails = poLines;
        })

        this.pmdataTransfer.itemSelectedChild$.subscribe(child => { this.selectedChild = child; });

    }


    updateWarehouseSelection(location: string = '') {
        let wid = this.warehouses.find(w => w.warehouse == this.pmDetails.warehouse)?.id;
        this.validLocations = this.locations.filter(l => l.warehouseId == wid);
        this.validLocationNames = this.validLocations.map(l => l.location);

        if (!location) { this.pmDetails.location = ''; }

    }

    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }
    pmDetails: pmDetails = new pmDetails();
    pmpoviewArray: PMPOView[] = [];
    invTypes: string[] = ["BASIC", "SERIAL"];
    warehouseLbl: string = "Warehouse";
    locationLbl: string = "Location";
    handleSubmit = function (e: any) {
        setTimeout(() => {
            alert("Submitted");
        }, 1000);

        e.preventDefault();
    }

}