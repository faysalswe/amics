import { Component, OnInit } from "@angular/core";
import { Guid } from "guid-typescript";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmSearchResult } from "src/app/pages/models/pmsearch";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";

@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {
    defaultWarehouse:string='';
    defaultLocation:string='';
    warehouses: Warehouse[] = [];
    warehouseNames:string[]=[];
    locations:WarehouseLocation[] =[];
    validLocations:WarehouseLocation[] =[];
    validLocationNames:string[]=[];
    constructor(private searchService: SearchService) { }

    ngOnInit(): void {
         
         this.searchService.getWarehouseInfo('').subscribe(w=>{
             this.warehouses = w;
             this.warehouseNames = w.map(w=>w.warehouse);
             this.defaultWarehouse = this.warehouseNames[0];
         })

         this.searchService.getLocationInfo('','').subscribe(l=>{
            this.locations = l;
        })
    }

    updateWarehouseSelection()  {
        let wid = this.warehouses.find(w=>w.warehouse == this.defaultWarehouse)?.id;
        this.validLocations = this.locations.filter(l=>l.warehouseId == wid);
        this.validLocationNames = this.validLocations.map(l=>l.location);
        this.defaultLocation = '';
      }
          
    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }
    pmDetails: pmDetails|undefined;
    pmpoviewArray: PMPOView[] =[];
    invTypes: string[] =["Basic" ,"Serial"];
    warehouseLbl:string = "Warehouse";
    locationLbl:string = "Location";
    handleSubmit = function (e:any) {
        setTimeout(() => {
            alert("Submitted");
        }, 1000);

        e.preventDefault();
    }

}