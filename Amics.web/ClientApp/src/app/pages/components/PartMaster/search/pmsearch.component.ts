import { Component, OnInit } from "@angular/core";
import { pmSearch, pmSearchResult } from "src/app/pages/models/pmsearch";
import { PartMasterService } from "../../../services/partmaster.service";

@Component({
    selector: "app-pmsearch",
    templateUrl: "./pmsearch.component.html",
    styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent {
    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }

    pmsearch: pmSearch | undefined;
    pmSearchResults: pmSearchResult[] = [];
    constructor(service: PartMasterService) { }
    handleSubmit = function (e:any) {
        setTimeout(() => {
            alert("Submitted");
        }, 1000);

        e.preventDefault();
    }
}