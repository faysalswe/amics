import { Component, OnInit } from "@angular/core"; 
import { pmSearch } from "src/app/pages/models/pmsearch";
import { Company, PartMasterService } from "../../../services/partmaster.service";

@Component({
  selector: "app-pmsearch",
  templateUrl: "./pmsearch.component.html",
  styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent  {
    pmsearch: pmSearch | undefined;
    constructor(service: PartMasterService) {}

}