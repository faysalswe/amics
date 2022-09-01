import { Component, Input, OnInit } from '@angular/core';
import notify from 'devextreme/ui/notify';
import { ComponentType } from '../../models/componentType';
import { LabelMap } from '../../models/Label';
import { mdatItemSearchResult, MdatSearch, SomainLookUp, StatusLookUp } from '../../models/mdatSearch';
import { BomAction } from '../../models/pmBomGridDetails';
import { CRUD } from '../../models/pmChildType';
import { pmCRUDActionType } from '../../models/pmCRUDActionType';
import { pmDetails } from '../../models/pmdetails';
import { MdatService } from '../../services/mdat.service';
import { MdatdatatransferService } from '../../services/mdatdatatransfer.service';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { TextboxStyle } from '../textbox-style/textbox-style';

@Component({
  selector: 'app-mdat',
  templateUrl: './mdat.component.html',
  styleUrls: ['./mdat.component.scss']
})
export class MdatComponent implements OnInit {
  @Input() componentType: ComponentType = ComponentType.Mdat;

  mdatSearchInfo: MdatSearch = new MdatSearch();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    width: "100%",
    type: "default",
  };

  mdatDetails: mdatItemSearchResult = new mdatItemSearchResult();
  selectedItemNumber: string = '';
  disabled = false;
  customers = [];
  pmSearchResults: mdatItemSearchResult[] = [];
  warehouseList = [];
  mdatList = [];
  now: Date = new Date();
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;

  erList: SomainLookUp[] = [];
  statusList: StatusLookUp[] = [];
  selectedItem: any;
  saveExitVisible = false;
  pmActions1: any[] = [{ "text": "Add" }, { "text": "Edit" }, { "text": "Delete" }];
  pmActions2: any[] = [{ "text": "Save" }, { "text": "Cancel" }];
  saveExitVisible2 = false;
  crudStatus = false;

  constructor(private mdatService: MdatService,
    private mdatDataTransService: MdatdatatransferService) {
    this.labelMap = LabelMap;
    this.search();
  }

  ngOnInit(): void {
    this.mdatDataTransService.selectedCRUD$.subscribe(crud => { 
      this.showSaveExit(crud)
      if(crud == CRUD.Add){
        this.mdatDetails = new mdatItemSearchResult();
        this.crudStatus = true;
      }
      else if(crud == CRUD.Edit){
        this.crudStatus = false;
      }
      else if(crud == CRUD.Save){
        this.onSave();
      } 
      else{
        this.search()
      }

    });
    this.mdatDataTransService.selectedItemForPMDetails$.subscribe((item) => {
      
      this.mdatDetails = item;
    });
    this.mdatService.getSomainLookUp('','', '8D3DBEC0-9E7F-490B-8B49-27F7DF81A74E').subscribe((result) => {
      this.erList = result.slice(0, 2000);
    })
    this.mdatService.getStatusLookUp('','').subscribe((result) => {
      
      this.statusList = result;
    })
    
  }

  dateHandling(e: any, type: string){
    switch(type){
      case 'submitted_date':
        this.mdatDetails.submitted_date = e?.value;
        break;
      case 'approved_date':
        this.mdatDetails.approved_date = e?.value;
        break;
      case 'cancelled_date':
        this.mdatDetails.cancelled_date = e?.value;
        break;
    }
  }

  onSelectionChanged(e: any) {
    if (!!e.addedItems[0]) {
      this.selectedItem = e.addedItems[0];
      this.selectedItemNumber = this.selectedItem.itemNumber;
      this.mdatDataTransService.selectedItemChanged(this.selectedItem, this.componentType);
    }
  }


  handleSubmit(e: any) {
    this.search();
    e.preventDefault();
  }

  search() {
    this.mdatService.getMdatSearchResults(this.mdatSearchInfo).subscribe(r => {
      debugger
      this.pmSearchResults = r;
      if (this.pmSearchResults.length !== 0) {
        this.mdatDataTransService.selectedItemChanged(this.pmSearchResults[0], this.componentType);
        this.selectedItem = this.pmSearchResults[0];
        this.selectedItemNumber = this.selectedItem.itemNumber;
      }
    });
  }

  onSave(){
    debugger
    if(this.crudStatus){
      this.mdatDetails.actionFlag = BomAction.Add;  
    }
    else{
      this.mdatDetails.actionFlag = BomAction.Update
    }
    this.mdatService.addorUpdateMDATDetails(this.mdatDetails).subscribe(
      (b) => {
        notify(
          { message: b.message, shading: true, position: top },
          'success',
          500
        );
      },
      (err) => {
        notify(
          { message: 'error while saving bom', shading: true },
          'error',
          1000
        );
      }
    );
  }

  mdatCRUDActionsSelectionChanged(e: any) {
    this.mdatDataTransService.selectedCRUD$.next(e);
    if(e == "Save"){

    }
  }

  showSaveExit(e: any) {
    if (e === pmCRUDActionType.Add || e === pmCRUDActionType.Edit) {
      this.saveExitVisible = true;
    }
    else {
      this.saveExitVisible = false;
    }
  }

}
