import { Component, OnInit, ViewChild } from '@angular/core';
import { DxDataGridComponent } from 'devextreme-angular';
import { ComponentType } from '../../models/componentType';
import { LabelMap } from '../../models/Label';
import { pmItemSearchResult, pmSearch } from '../../models/pmsearch';
import { pmSerial } from '../../models/pmSerial';
import { PartMasterService } from '../../services/partmaster.service';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { SearchService } from '../../services/search.service';
import { TextboxStyle } from '../textbox-style/textbox-style';
import { changeSerial } from 'src/app/pages/models/pmSerial';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-change-serial',
  templateUrl: './change-serial.component.html',
  styleUrls: ['./change-serial.component.scss']
})
export class ChangeSerialComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false }) dataGrid!: DxDataGridComponent
  changeSerialSearchInfo: changeSerial = new changeSerial();
  secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
  popupVisible = false;
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;
  SerialId: Guid = Guid.createEmpty();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    type: "default"

  };
  submitSerialPopupButtonOptions = {
    text: "Save and exit",
    useSubmitBehavior: true,
    type: "default"
};

cancelSerialPopupButtonOptions = {
    text: "Cancel and exit",
    useSubmitBehavior: true,
    type: "default"

};

  changesserialArray: pmSerial[] = [];
  changeserialOptions = [
    'Ware house',
    'Location',
    'Cost',
    'Serial',
    'Tag No',
    'Model',
    'Exp Date',
    'Original year',
    'Current year'
  ];

  rowSelection: boolean = false;

  constructor(
    private pmService: PartMasterService,
    private searchService: SearchService
    ) 
    {
      this.labelMap = LabelMap;
    }

  ngOnInit(): void {
  }

  handleSubmit(e: any) {

    e.preventDefault();
    this.rowSelection = false;
    let searchDto = new pmSearch();
    let ItemsId: string;
    searchDto.itemnumber = this.changeSerialSearchInfo.itemNumber;
    this.searchService.getItemNumberSearchResults(searchDto).subscribe((response) => {
      let result = response;
      ItemsId = String(result?.find(x => x.itemNumber == this.changeSerialSearchInfo.itemNumber)?.id);
      this.pmService.getViewSerial(ItemsId, this.secUserId).subscribe((res) => {
        debugger
        this.changesserialArray = res;

        if (this.changesserialArray.length > 0) {
          
          this.changeSerialSearchInfo.serNoFm = this.changesserialArray[0].serlot;
          this.changeSerialSearchInfo.serNoTo = this.changesserialArray[0].serlot;
          this.changeSerialSearchInfo.tagNoFm = this.changesserialArray[0].tagcol;
          this.changeSerialSearchInfo.tagNoTo = this.changesserialArray[0].tagcol;
        }

      });
    });
  }

  saveSerial(e: any){
    debugger
    this.changeSerialSearchInfo.costFm = this.changeSerialSearchInfo.costFm.toString();
    this.changeSerialSearchInfo.costTo = this.changeSerialSearchInfo.costTo.toString();
    this.pmService.updateChangeSerialTag(this.changeSerialSearchInfo)
        .subscribe(response => {
          this.popupVisible = false;
          alert(response);
        });

    e.preventDefault();
    //alert("save clicked")
  }

  edit() {
    this.popupVisible = true;
  }

  onRowSelection(e: any) {
    let selectedRow = e.data;
    this.rowSelection = true;
    this.changeSerialSearchInfo.serNoFm = selectedRow?.serlot;
    this.changeSerialSearchInfo.serNoTo = selectedRow?.serlot;
    this.changeSerialSearchInfo.tagNoFm = selectedRow?.tagcol;
    this.changeSerialSearchInfo.tagNoTo = selectedRow?.tagcol;
    this.changeSerialSearchInfo.modelFm = selectedRow?.color_model;
    this.changeSerialSearchInfo.modelTo = selectedRow?.color_model;
    this.changeSerialSearchInfo.costFm = selectedRow?.cost;
    this.changeSerialSearchInfo.costTo = selectedRow?.cost;

    this.changeSerialSearchInfo.serialId = selectedRow?.id;
    this.popupVisible = true;
  }

}

// export class changeSerialInfo {
//   itemNumber: string = "";
//   fromSerial: string = "";
//   toSerial: string = "";
//   fromTagNo: string = "";
//   toTagNo: string = "";
//   fromModel: string = "";
//   toModel: string = "";
//   fromCost: string = "";
//   toCost: string = "";
// }
