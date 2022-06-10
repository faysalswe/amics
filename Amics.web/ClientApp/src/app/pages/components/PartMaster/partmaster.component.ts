import { Component, OnInit } from "@angular/core";
import { Company } from "../../models/company";
import { ComponentType } from "../../models/componentType";
import { ItemClass, ItemCode, ItemType } from "../../models/searchModels";
import { PartMasterService } from "../../services/partmaster.service";

@Component({
  selector: "app-partmaster",
  templateUrl: "./partmaster.component.html",
  styleUrls: ['./partmaster.component.scss']
})
export class PartMasterComponent {
  companies: Company[];
  componentType:ComponentType = ComponentType.PartMaster;

  labelMode: string;
  labelLocation: string;
  readOnly: boolean;
  showColon: boolean;
  minColWidth: number;
  colCount: number;
  width: any;
  saveExitVisible = false;

  itemClassList: ItemClass[] = [];
  itemCodeList: ItemCode[] = [];
  itemTypeList: ItemType[] = [];
  children: string[] = ["BOM", "PO", "Notes", "Pictures", "Documents"];
  selctedChild: string = "BOM";
  constructor(service: PartMasterService) {
    this.labelMode = 'static';
    this.labelLocation = 'left';
    this.readOnly = false;
    this.showColon = true;
    this.minColWidth = 300;
    this.colCount = 2;
    this.companies = service.getCompanies();
  }
  pmActions1: any[] = [{ "text": "Add" }, { "text": "Edit" }, { "text": "Delete" }];
  pmActions2: any[] = [{ "text": "Save" }, { "text": "Cancel" }];
  saveExitVisible2 = false;
  getCompanySelectorLabelMode() {
    return this.labelMode === 'outside'
      ? 'hidden'
      : this.labelMode;
  }

  logpmActionsSelectionChanged(e: any) {
    if (e === "Add" || e === "Edit") {
      this.saveExitVisible = true;
    }
    else {
      this.saveExitVisible = false;
    }

  }

  logpmActions2SelectionChanged(e: any) {


  }

}
