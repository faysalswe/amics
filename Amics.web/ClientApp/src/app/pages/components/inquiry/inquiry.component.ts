import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-inquiry',
  templateUrl: './inquiry.component.html',
  styleUrls: ['./inquiry.component.scss'],
})
export class InquiryComponent implements OnInit {
   inventoryArray = []
  isCost:boolean = false;
  inquiryOptions = [
    'Part Number',
    'Serial #',
    'Tag #',
    'ER',
    'Location',
    'Description',
    'MDAT In',
  ];
  searchLabel: string = 'Part Number';
  constructor() {}

  ngOnInit(): void {}

  onValueChanged($event: any) {
    this.searchLabel = $event.value;
  }

  changeCost($event: any){
    this.isCost = $event.value
  }
}
