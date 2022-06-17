import { Component, OnInit } from '@angular/core';
import { InquiryActionType, InquiryRequest, InquiryResponse } from '../../models/inquiryRequest';
import { PartMasterService } from '../../services/partmaster.service';

@Component({
  selector: 'app-inquiry',
  templateUrl: './inquiry.component.html',
  styleUrls: ['./inquiry.component.scss'],
})
export class InquiryComponent implements OnInit {

  inventoryArray = []
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
  inquiryMapOptions = new Map<string, InquiryActionType>();
  searchLabel: string = 'Part Number';
  searchText: string = '';
  action: InquiryActionType = InquiryActionType.PartMaster;
  inquiryResponseDetails: InquiryResponse[] = [];
  inquiryRequest: InquiryRequest = new InquiryRequest();
  constructor(private pmService: PartMasterService) {

  }

  ngOnInit(): void { }

  onValueChanged($event: any) {
    this.searchLabel = $event.value as string;
    var res = this.dictionaryActions.find(d => d.name === this.searchLabel);
    this.action = !!res ? res.action : InquiryActionType.PartMaster;
  }

  changeCost($event: any): void {
    this.isCost = $event.value
  }
  searchInquiryDetails() {
    this.inquiryRequest.action = this.action;
    this.inquiryRequest.searchText = this.searchText;
    this.inquiryRequest.user = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
    this.pmService.getInquiryDetails(this.inquiryRequest).subscribe(response => {
      this.inquiryResponseDetails = response;
      console.log(response);
    });   
  }
}
