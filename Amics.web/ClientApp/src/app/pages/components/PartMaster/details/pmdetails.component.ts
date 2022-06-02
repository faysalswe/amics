import { Component, OnInit } from "@angular/core";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmSearchResult } from "src/app/pages/models/pmsearch";
import { warehouse } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";

@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {

    warehouses: warehouse[] = [];
    locations:Location[] =[];
    constructor(private searchService: SearchService) { }

    ngOnInit(): void {
         
         this.searchService.getWarehouseInfo('').subscribe(w=>{
             this.warehouses = w;
         })

         this.searchService.getLocationInfo('','').subscribe(l=>{
            this.locations = l;
        })
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