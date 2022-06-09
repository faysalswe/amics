import { Injectable } from "@angular/core";
import { BehaviorSubject, filter, forkJoin, of, Subject, switchMap } from "rxjs";
import { pmDetails } from "../../models/pmdetails";
import { pmItemSearchResult } from "../../models/pmsearch";
import { PartMasterService } from "../../services/partmaster.service";

@Injectable({
    providedIn: "root",
})
export class PartMasterDataTransService {

    constructor(private pmService: PartMasterService) { }

    private itemSelectedSubject$ = new Subject<pmItemSearchResult>();

    selectedItemChanged(selectedProductId: pmItemSearchResult): void {
        this.itemSelectedSubject$.next(selectedProductId);
    }

    selectedItem$ = this.itemSelectedSubject$.pipe(switchMap(i => this.pmService.getPartMaster(i.itemNumber, i.rev)))
}