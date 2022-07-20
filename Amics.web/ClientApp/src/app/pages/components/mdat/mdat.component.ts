import { Component, OnInit } from '@angular/core';
import { LabelMap } from '../../models/Label';
import { pmDetails } from '../../models/pmdetails';
import { TextboxStyle } from '../textbox-style/textbox-style';

@Component({
  selector: 'app-mdat',
  templateUrl: './mdat.component.html',
  styleUrls: ['./mdat.component.scss']
})
export class MdatComponent implements OnInit {

  customers = [];
  pmSearchResults = [];
  warehouseList = [];
  mdatList = [];
  now: Date = new Date();
  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;
  constructor() {
    this.labelMap = LabelMap;
  }

  ngOnInit(): void {
  }

  onSelectionChanged($event: any) {

  }

}
