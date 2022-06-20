import { Component } from "@angular/core";
import { PartMasterDataTransService } from "src/app/pages/services/pmdatatransfer.service";

@Component({
    selector: "app-pmViewLocation",
    templateUrl: "./ViewLocation.component.html",
    styleUrls: ['./viewLocation.component.scss']
})
export class PMSearchComponent {

    constructor( private pmDataTransService: PartMasterDataTransService)
    {
        
    }
}