import { Injectable } from "@angular/core";
import { Subject, switchMap } from "rxjs";
import { ComponentType } from "../models/componentType";
import { PmChildType } from "../models/pmChildType"; 
import { pmItemSearchResult } from "../models/pmsearch";
import { PartMasterService } from "./partmaster.service";

@Injectable({
    providedIn: "root",
})
export class PartMasterDataTransService {

    constructor(private pmService: PartMasterService) { }

    private itemSelectedSubjectForPMScreen$ = new Subject<pmItemSearchResult>();
    public itemSelectedChild$ = new Subject<PmChildType>();

    selectedItemChanged(selectedProductId: pmItemSearchResult, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedProductId) {
            this.itemSelectedSubjectForPMScreen$.next(selectedProductId);
        }
    }
    }
    selectedChildChanged(selectedChild: PmChildType, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedChild) {
            this.itemSelectedChild$.next(selectedChild);
        }
    }
    }
    selectedItemForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
    selectedItemBomForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getBomDetails(i.id)))
    selectedItemPoForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPoDetails(i.id)))
}