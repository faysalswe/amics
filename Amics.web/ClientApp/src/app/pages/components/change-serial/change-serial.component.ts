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

@Component({
  selector: 'app-change-serial',
  templateUrl: './change-serial.component.html',
  styleUrls: ['./change-serial.component.scss']
})
export class ChangeSerialComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false }) dataGrid!: DxDataGridComponent
  changeSerialSearchInfo: changeSerialInfo = new changeSerialInfo();
  secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
  popupVisible = false;
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    type: "default"

  };
  submitPopupButtonOptions = {
    text: "Save",
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
        this.changesserialArray = res;

        if (this.changesserialArray.length > 0) {
          
          this.changeSerialSearchInfo.fromSerial = this.changesserialArray[0].serlot;
          this.changeSerialSearchInfo.toSerial = this.changesserialArray[0].serlot;
          this.changeSerialSearchInfo.fromTagNo = this.changesserialArray[0].tagcol;
          this.changeSerialSearchInfo.toTagNo = this.changesserialArray[0].tagcol;
        }

      });
    });
  }

  saveSerial(e: any){

  }

  edit() {
    this.popupVisible = true;
  }

  onRowSelection(e: any) {
    let selectedRow = e.data;
    this.rowSelection = true;
    this.changeSerialSearchInfo.fromSerial = selectedRow?.serlot;
    this.changeSerialSearchInfo.toSerial = selectedRow?.serlot;
    this.changeSerialSearchInfo.fromTagNo = selectedRow?.tagcol;
    this.changeSerialSearchInfo.toTagNo = selectedRow?.tagcol;
    this.changeSerialSearchInfo.fromModel = selectedRow?.color_model;
    this.changeSerialSearchInfo.toModel = selectedRow?.color_model;
    this.changeSerialSearchInfo.fromCost = selectedRow?.cost;
    this.changeSerialSearchInfo.toCost = selectedRow?.cost;
    this.popupVisible = true;
  }

}

export class changeSerialInfo {
  itemNumber: string = "";
  fromSerial: string = "";
  toSerial: string = "";
  fromTagNo: string = "";
  toTagNo: string = "";
  fromModel: string = "";
  toModel: string = "";
  fromCost: string = "";
  toCost: string = "";
}
