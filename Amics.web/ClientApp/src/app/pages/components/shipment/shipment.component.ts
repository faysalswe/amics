import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-shipment',
  templateUrl: './shipment.component.html',
  styleUrls: ['./shipment.component.scss']
})
export class ShipmentComponent {
  gridList = [];
  pmSearchResults = [
    { itemNumber: "Blue", description: "This is a blue color" },
    { itemNumber: "Red", description: "This is a red color" },
    { itemNumber: "Orange", description: "This is a orange color" },
    { itemNumber: "Green", description: "This is a green color" },
  ];
  selectedItemNumber: string = '';
  statusList = [];
  locationList = [];

  tableRight: Array<Shipment> = [];
  tableLeft: Array<Shipment> = [
    { wareHouse: "wareHouse 1", location: "Newyork", serialNo: "11", tagNo: "tag-1", qty: "111" },
    { wareHouse: "wareHouse 2", location: "Texas", serialNo: "11", tagNo: "tag-1", qty: "111" },
    { wareHouse: "wareHouse 3", location: "Connictcut", serialNo: "11", tagNo: "tag-1", qty: "111" },
    { wareHouse: "wareHouse 4", location: "New jersey", serialNo: "11", tagNo: "tag-1", qty: "111" },
    { wareHouse: "wareHouse 5", location: "california", serialNo: "11", tagNo: "tag-1", qty: "111" },
  ];

  constructor() {
    this.onAdd = this.onAdd.bind(this);
  }

  onAdd(event: any) {
    debugger

    let rowData = new Shipment();
    let itemData = event.itemData;
    rowData.wareHouse = itemData.wareHouse;
    rowData.location = itemData.location;
    rowData.serialNo = itemData.serialNo;
    rowData.tagNo = itemData.tagNo;
    rowData.qty = itemData.qty;

    let element = event.element.parentElement.className;

    if (element == 'column1') {
      if(this.tableLeft[event.toIndex] !== undefined){
        this.tableLeft.splice(event.toIndex, 0, rowData);
      }
      else{
        this.tableLeft.push(rowData);
      }
      this.tableRight.splice(event.fromIndex, 1);
    }
    else if (element == 'column2') {
      if(this.tableRight[event.toIndex] !== undefined){
        this.tableRight.splice(event.toIndex, 0, rowData);
      }
      else{
        this.tableRight.push(rowData);
      }
      this.tableLeft.splice(event.fromIndex, 1);
    }
  }
}
export class Shipment {
  wareHouse: string = "";
  location: string = "";
  serialNo: string = "";
  tagNo: string = "";
  qty: string = "";
}