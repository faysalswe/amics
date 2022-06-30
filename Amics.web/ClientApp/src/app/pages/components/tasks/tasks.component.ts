import { Component, OnInit } from '@angular/core';
import 'devextreme/data/odata/store';
import { taskItemSearchResult } from '../../models/pmsearch';

@Component({
  selector: "app-tasks",
  templateUrl: 'tasks.component.html',
  styleUrls: ['tasks.component.scss']
})

export class TasksComponent {

  gridList = [];
  pmSearchResults = [
    { itemNumber: "Car", description: "This is a car" },
    { itemNumber: "Bike", description: "This is a bike" },
    { itemNumber: "Aeroplane", description: "This is a aeroplane" },
    { itemNumber: "Train", description: "This is a train" },
  ];
  selectedItemNumber: string = '';
  statusList = [];
  locationList = [];

  tableRight: Array<Task> = [];
  tableLeft: Array<Task> = [
    { wareHouse: "house 1", location: "England", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 2", location: "France", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 3", location: "America", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 4", location: "Russia", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 5", location: "Pakistan", serialNo: "11", tagNo: "tag-66", qty: "111" },
  ];

  constructor() {
    this.onAdd = this.onAdd.bind(this);
  }

  onAdd(event: any) {
    debugger

    let rowData = new Task();
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

export class Task {
  wareHouse: string = "";
  location: string = "";
  serialNo: string = "";
  tagNo: string = "";
  qty: string = "";
}