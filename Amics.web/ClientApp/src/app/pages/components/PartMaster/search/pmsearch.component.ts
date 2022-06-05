import { Component, OnInit } from "@angular/core";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { ItemClass, ItemCode, ItemType } from "src/app/pages/models/searchModels";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";

@Component({
    selector: "app-pmsearch",
    templateUrl: "./pmsearch.component.html",
    styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent implements OnInit {
    pmsearchInfo: pmSearch = new pmSearch();
    pmSearchResults: pmItemSearchResult[] = []; 
    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }; 
    itemClassList: ItemClass[] = [];
    itemCodeList: ItemCode[] = [];
    itemTypeList: ItemType[] = [];
 
    constructor(private searchService: SearchService) { }

    ngOnInit(): void {
        this.searchService.getItemClass('', '').subscribe(l => {
            this.itemClassList = l; 
        })

        this.searchService.getItemCode('', '').subscribe(l => {
            this.itemCodeList = l; 
        })

        this.searchService.getItemType('', '').subscribe(l => {
            this.itemTypeList = l; 
        }) 
    }


    handleSubmit(e: any) {
        console.log(this.pmsearchInfo);
        this.searchService.getItemNumberSearchResults(this.pmsearchInfo).subscribe(r =>
            this.pmSearchResults = r);

        e.preventDefault();
    }

}