import { Component, OnInit } from '@angular/core';
import { InquiryActionType, InquiryRequest, InquiryResponse } from '../../models/inquiryRequest';
import { LabelMap } from '../../models/Label';
import { PartMasterService } from '../../services/partmaster.service';
import { TextboxStyle } from '../textbox-style/textbox-style';

@Component({
  selector: 'app-inquiry',
  templateUrl: './inquiry.component.html',
  styleUrls: ['./inquiry.component.scss'],
})
export class InquiryComponent implements OnInit {

  isCost: boolean = false;
  dictionaryActions = [
    { name: 'Part Number', action: InquiryActionType.PartMaster },
    { name: 'Serial #', action: InquiryActionType.Serial },
    { name: 'Tag #', action: InquiryActionType.Tag },
    { name: 'ER', action: InquiryActionType.ER },
    { name: 'Location', action: InquiryActionType.Location },
    { name: 'Description', action: InquiryActionType.Description },
    { name: 'MDAT In', action: InquiryActionType.MdatIn }
  ];
  inquiryOptions = [
    'Part Number',
    'Serial #',
    'Tag #',
    'ER',
    'Location',
    'Description',
    'MDAT In',
  ];

  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;
  inquiryMapOptions = new Map<string, InquiryActionType>();
  searchLabel: string = 'Part Number';
  textBoxLabel: number = 101;
  searchText: string = '';
  action: InquiryActionType = InquiryActionType.PartMaster;
  inquiryResponseDetails: InquiryResponse[] = [];
  inquiryRequest: InquiryRequest = new InquiryRequest();
  constructor(private pmService: PartMasterService) {

    this.labelMap = LabelMap;
  }

  ngOnInit(): void { }

  onValueChanged($event: any) {
    this.searchLabel = $event.value as string;
    var res = this.dictionaryActions.find(d => d.name === this.searchLabel);
    this.action = !!res ? res.action : InquiryActionType.PartMaster;
    this.textBoxLabel = this.findLabelNumber(this.searchLabel);
  }

  changeCost($event: any): void {
    this.isCost = $event.value
  }
  searchInquiryDetails() {
    this.inquiryRequest.action = this.action;
    this.inquiryRequest.searchText = this.searchText;
    this.inquiryRequest.user = 'admin';
    this.pmService.getInquiryDetails(this.inquiryRequest).subscribe(response => {
      this.inquiryResponseDetails = response;
      console.log(response);
    });
  }


  findLabelNumber(label: string): number {
    switch (label) {
      case 'Part Number':
        return this.labelMap.partNumber_num;
      case 'Serial #':
        return this.labelMap.serialNo_num;
      case 'Tag #':
        return this.labelMap.tagNo_num;
      case 'ER':
        return this.labelMap.er_num;
      case 'Location':
        return this.labelMap.location_num;
      case 'Description':
        return this.labelMap.description_num;
      case 'MDAT In':
        return this.labelMap.mdatIn_num;
      default:
        return this.labelMap.partNumber_num;
    }
  }
}
