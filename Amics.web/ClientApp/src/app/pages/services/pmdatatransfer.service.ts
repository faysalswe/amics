import { Injectable } from "@angular/core";
import { Subject, switchMap } from "rxjs";
import { ComponentType } from "../models/componentType";
import { CRUD, PmChildType } from "../models/pmChildType";
import { pmItemSearchResult } from "../models/pmsearch";
import { PartMasterService } from "./partmaster.service";

@Injectable({
    providedIn: "root",
})
export class PartMasterDataTransService {

    constructor(private pmService: PartMasterService) { }

    private itemSelectedSubjectForPMScreen$ = new Subject<pmItemSearchResult>();
    public itemSelectedChild$ = new Subject<PmChildType>();
    public itemSelectedCRUD$ = new Subject<CRUD>();

    selectedItemChanged(selectedProductId: pmItemSearchResult, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForPMScreen$.next(selectedProductId);
                this.itemSelectedCRUD$.next(CRUD.Edit);
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

    selectedCRUDActionChanged(selectedCRUD: CRUD, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedCRUD) {
                this.itemSelectedCRUD$.next(selectedCRUD);
            }
        }
    }

    selectedItemForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
    selectedItemBomForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getBomDetails(i.id)))
    selectedItemPoForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPoDetails(i.id)))
}
