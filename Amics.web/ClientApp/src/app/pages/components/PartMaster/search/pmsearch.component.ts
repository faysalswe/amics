import { Component, OnInit } from "@angular/core";
import { pmSearch, pmSearchResult } from "src/app/pages/models/pmsearch";
import { warehouse } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";

@Component({
    selector: "app-pmsearch",
    templateUrl: "./pmsearch.component.html",
    styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent implements OnInit {
 
    constructor(private searchService: SearchService) { }

    ngOnInit(): void {
         
    }
    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }

    pmsearch: pmSearch | undefined;
    pmSearchResults: pmSearchResult[] = [];

    handleSubmit = function (e: any) {
        setTimeout(() => {
            alert("Submitted");
        }, 1000);

        e.preventDefault();
    }
}