import { Component, OnInit } from "@angular/core";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../pmdatatransfer.service";

@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {
    defaultWarehouse: string = '';
    defaultLocation: string = '';
    warehouses: Warehouse[] = [];
    warehouseNames: string[] = [];
    locations: WarehouseLocation[] = [];
    validLocations: WarehouseLocation[] = [];
    validLocationNames: string[] = [];
    invType: string = '';
    constructor(private searchService: SearchService, private pmdataTransfer: PartMasterDataTransService) { }

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
            this.defaultWarehouse = item.warehouse;
            this.defaultLocation='';
            this.updateWarehouseSelection(item.location);
        });
    }

    updateWarehouseSelection(location: string = '') {
        let wid = this.warehouses.find(w => w.warehouse == this.defaultWarehouse)?.id;
        this.validLocations = this.locations.filter(l => l.warehouseId == wid);
        this.validLocationNames = this.validLocations.map(l => l.location);
        this.defaultLocation = '';
        if (!!location) { this.defaultLocation = location; }
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