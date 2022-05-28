import { Component, OnInit } from "@angular/core"; 
import { Company } from "../../models/company";
import { PartMasterService } from "../../services/partmaster.service";

@Component({
  selector: "app-partmaster",
  templateUrl: "./partmaster.component.html",
  styleUrls: ['./partmaster.component.scss']
})
export class PartMasterComponent  {
  companies: Company[];

  labelMode: string;
  labelLocation: string;
  readOnly: boolean;
  showColon: boolean;
  minColWidth: number;
  colCount: number;
  width: any;

  constructor(service: PartMasterService) {
    this.labelMode = 'static';
    this.labelLocation = 'left';
    this.readOnly = false;
    this.showColon = true;
    this.minColWidth = 300;
    this.colCount = 2;
    this.companies = service.getCompanies();
  }
   

  getCompanySelectorLabelMode() {
    return this.labelMode === 'outside'
      ? 'hidden'
      : this.labelMode;
  }
}
