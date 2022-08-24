import { Component, OnInit } from '@angular/core';
import { LabelMap } from '../../models/Label';
import { mdatItemSearchResult, MdatSearch } from '../../models/mdatSearch';
import { pmDetails } from '../../models/pmdetails';
import { MdatService } from '../../services/mdat.service';
import { TextboxStyle } from '../textbox-style/textbox-style';

@Component({
  selector: 'app-mdat',
  templateUrl: './mdat.component.html',
  styleUrls: ['./mdat.component.scss']
})
export class MdatComponent implements OnInit {

  mdatSearchInfo: MdatSearch = new MdatSearch();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    width: "100%",
    type: "default",
};

  customers = [];
  pmSearchResults:mdatItemSearchResult[] = [];
  warehouseList = [];
  mdatList = [];
  now: Date = new Date();
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;

  constructor(private mdatService: MdatService) {
    this.labelMap = LabelMap;
    this.search();
  }

  ngOnInit(): void {
  }

  onSelectionChanged($event: any) {

  }

  handleSubmit(e: any) {
    this.search();
    e.preventDefault();
}

  search() {
    debugger
    this.mdatService.getMdatSearchResults(this.mdatSearchInfo).subscribe(r => {
        this.pmSearchResults = r;
        if (this.pmSearchResults.length !== 0) {
            // this.pmDataTransService.selectedItemChanged(this.pmSearchResults[0], this.componentType);
            // this.selectedItem = this.pmSearchResults[0];
            // this.selectedItemNumber = this.selectedItem.itemNumber;
        } 
    });
}

}
