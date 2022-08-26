import { Injectable } from '@angular/core';
import { Subject, switchMap } from 'rxjs';
import { ComponentType } from '../models/componentType';
import { mdatItemSearchResult } from '../models/mdatSearch';
import { CRUD } from '../models/pmChildType';
import { MdatService } from './mdat.service';

@Injectable({
  providedIn: 'root'
})
export class MdatdatatransferService {

private itemSelectedSubjectForPMScreen$ = new Subject<mdatItemSearchResult>();
public selectedCRUD$ = new Subject<CRUD>();

constructor(private mdatService: MdatService) { }


selectedItemChanged(selectedProductId: mdatItemSearchResult, componentType: ComponentType): void {
  if (componentType === ComponentType.Mdat) {
      if (!!selectedProductId) {
          this.itemSelectedSubjectForPMScreen$.next(selectedProductId);                
      }
  }
}

selectedCRUDActionChanged(selectedCRUD: CRUD, componentType: ComponentType): void {
  if (componentType === ComponentType.Mdat) {
      if (!!selectedCRUD) {
          this.selectedCRUD$.next(selectedCRUD);
      }
  }
}

selectedItemForPMDetails$ = this.itemSelectedSubjectForPMScreen$.pipe(switchMap(i => this.mdatService.getMdatViewDetails(i.mdatNum)))


}


