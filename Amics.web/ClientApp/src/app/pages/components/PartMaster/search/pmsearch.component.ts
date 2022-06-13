import { Component, Input, OnInit } from "@angular/core";
import { ComponentType } from "src/app/pages/models/componentType";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { ItemClass, ItemCode, ItemType } from "src/app/pages/models/searchModels";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";

@Component({
    selector: "app-pmsearch",
    templateUrl: "./pmsearch.component.html",
    styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent implements OnInit {
    @Input() componentType: ComponentType = ComponentType.PartMaster;
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

    constructor(private searchService: SearchService, private pmDataTransService:PartMasterDataTransService) { }

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

    onSelectionChanged(e: any) {
        console.log(e);
        var selectedItem = e.selectedRowsData[0];
        console.log(selectedItem);
      
        this.pmDataTransService.selectedItemChanged(selectedItem,this.componentType);
    }
}