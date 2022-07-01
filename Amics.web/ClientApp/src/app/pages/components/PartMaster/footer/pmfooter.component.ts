import { Component, Input, OnInit } from "@angular/core";
import { ComponentType } from "src/app/pages/models/componentType";
import { pmCRUDActionType } from "src/app/pages/models/pmCRUDActionType";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";

@Component({
  selector: "app-pmfooter",
  templateUrl: "./pmfooter.component.html",
  styleUrls: ["./pmfooter.component.scss"]
})
export class PMFooterComponent {
  @Input() componentType: ComponentType = ComponentType.PartMaster;

  width: any;
  saveExitVisible = false;
  showViewSN = false;
  pmActions1: any[] = [{ "text": "Add" }, { "text": "Edit" }, { "text": "Delete" }];
  pmActions2: any[] = [{ "text": "Save" }, { "text": "Cancel" }];
  saveExitVisible2 = false;
  children: string[] = ["BOM", "PO", "Notes"];
  selctedChild: string = "BOM";
  constructor(private pmDataTranserService: PartMasterDataTransService) {
    this.pmDataTranserService.selectedCRUD$.subscribe(crud => { this.showSaveExit(crud) });
    this.pmDataTranserService.isSerialSelected$.subscribe(isSer => { this.showViewSN = isSer });
  }

  copyToNew(e: any) {
    this.pmDataTranserService.copyToNewSelected$.next(e);
  }
  pmCRUDActionsSelectionChanged(e: any) {
    this.pmDataTranserService.selectedCRUD$.next(e);
  }
  showSaveExit(e: any) {
    if (e === pmCRUDActionType.Add || e === pmCRUDActionType.Edit) {
      this.saveExitVisible = true;
    }
    else {
      this.saveExitVisible = false;
    }
  }


  logpmActions2SelectionChanged(e: any) {
    this.pmDataTranserService.selectedPopUpAction$.next(e);

  }
  handleValueChange(e: any) {
    const previousValue = e.previousValue;
    const newValue = e.value;
    this.pmDataTranserService.selectedChildChanged(e.value, this.componentType);
  };
}
