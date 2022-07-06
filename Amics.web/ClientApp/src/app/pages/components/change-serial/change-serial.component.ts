import { Component, OnInit } from '@angular/core';
import { ComponentType } from '../../models/componentType';
import { pmItemSearchResult, pmSearch } from '../../models/pmsearch';
import { PartMasterDataTransService } from '../../services/pmdatatransfer.service';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'app-change-serial',
  templateUrl: './change-serial.component.html',
  styleUrls: ['./change-serial.component.scss']
})
export class ChangeSerialComponent implements OnInit {
  changeSerialSearchInfo: changeSerialInfo = new changeSerialInfo();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    type: "default"

  };
  changesserialArray = [];
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
  constructor(
    private pmDataTransService: PartMasterDataTransService,
    private pmdataTransfer: PartMasterDataTransService,
    private searchService: SearchService) { }

  ngOnInit(): void {
  }

  handleSubmit(e: any) {
    debugger
    // console.log(e);
    e.preventDefault();

    let searchDto = new pmSearch();
    searchDto.itemnumber = this.changeSerialSearchInfo.itemNumber;
    let result = this.searchService.getItemNumberSearchResults(searchDto);
    // if (!!e.addedItems[0]) {
    //   this.selectedItem = e.addedItems[0];
    //   console.log(this.selectedItem);
    //   this.selectedItemNumber = this.selectedItem.itemNumber;
    //   this.pmDataTransService.selectedItemChanged(this.selectedItem, this.componentType);
    // }
  }

}

export class changeSerialInfo {
  itemNumber: string = "";
}
