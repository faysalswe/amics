import { Injectable } from "@angular/core";
import { BehaviorSubject, filter, forkJoin, of, Subject, switchMap } from "rxjs";
import { ComponentType } from "../../models/componentType";
import { pmDetails } from "../../models/pmdetails";
import { pmItemSearchResult } from "../../models/pmsearch";
import { PartMasterService } from "../../services/partmaster.service";

@Injectable({
    providedIn: "root",
})
export class PartMasterDataTransService {

    constructor(private pmService: PartMasterService) { }

    private itemSelectedSubjectForPMScreen$ = new Subject<pmItemSearchResult>();

    selectedItemChanged(selectedProductId: pmItemSearchResult, componentType: ComponentType): void {
        if (componentType === ComponentType.PartMaster) {
            this.itemSelectedSubjectForPMScreen$.next(selectedProductId);
        }
    }

    selectedItemForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
}