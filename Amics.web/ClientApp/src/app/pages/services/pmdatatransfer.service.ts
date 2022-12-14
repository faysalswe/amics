import { Injectable } from "@angular/core";
import { Subject, switchMap } from "rxjs";
import { ComponentType } from "../models/componentType";
import { CRUD, PmChildType, PopUpAction } from "../models/pmChildType";
import { pmItemSearchResult } from "../models/pmsearch";
import { PartMasterService } from "./partmaster.service";

@Injectable({
    providedIn: "root",
})
export class PartMasterDataTransService {

    constructor(private pmService: PartMasterService) { }

    private itemSelectedSubjectForPMScreen$ = new Subject<pmItemSearchResult>();
    public itemSelectedSubjectForBomGrid$ = new Subject<pmItemSearchResult>();
    private itemSelectedSubjectForIncInvScreen$ = new Subject<pmItemSearchResult>();
    private itemSelectedSubjectForDecInvScreen$ = new Subject<pmItemSearchResult>();
    public itemSelectedChild$ = new Subject<PmChildType>();
    public selectedCRUD$ = new Subject<CRUD>();
    public selectedPopUpAction$ = new Subject<PopUpAction>();
    public copyToNewSelected$ = new Subject<any>();
    public isSerialSelected$ = new Subject<any>();

    selectedItemChanged(selectedProductId: pmItemSearchResult, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForPMScreen$.next(selectedProductId);                
            }
        }
        else if (componentType === ComponentType.IncreaseInventory) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForIncInvScreen$.next(selectedProductId);
            }
        }
        else if (componentType === ComponentType.DecreaseInventory) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForDecInvScreen$.next(selectedProductId);
            }
        }
        else if (componentType === ComponentType.PartMasterF2) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForBomGrid$.next(selectedProductId);
            }
        }
        else if (componentType === ComponentType.ChangeSerial) {
            if (!!selectedProductId) {
                this.itemSelectedSubjectForBomGrid$.next(selectedProductId);
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
                this.selectedCRUD$.next(selectedCRUD);
            }
        }
    }
    selectedPopUpActionChanged(selectedAction: PopUpAction, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            if (!!selectedAction) {
                this.selectedPopUpAction$.next(selectedAction);
            }
        }
    }
    selectedItemForInvDetails$ = this.itemSelectedSubjectForIncInvScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
    selectedItemForDecInvDetails$ = this.itemSelectedSubjectForDecInvScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
    selectedItemForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
    selectedItemBomForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getBomDetails(i.id)))
    selectedItemPoForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPoDetails(i.id)))
    selectedItemNotesForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getNotes(i.id)))
}
