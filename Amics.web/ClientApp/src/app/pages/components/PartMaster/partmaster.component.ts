import { Component } from "@angular/core";
import { ComponentType } from "../../models/componentType";
import { PartMasterService } from "../../services/partmaster.service";

@Component({
  selector: "app-partmaster",
  templateUrl: "./partmaster.component.html",
  styleUrls: ['./partmaster.component.scss']
})
export class PartMasterComponent {
  componentType: ComponentType = ComponentType.PartMaster;

  constructor(service: PartMasterService) {
  }

}
