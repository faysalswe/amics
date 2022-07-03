import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import 'devextreme/data/odata/store';
import notify from 'devextreme/ui/notify';
import { changeLocProjDetails, changeLocRequest, changeLocSearchResult } from '../../models/changeLoc';
import { taskItemSearchResult } from '../../models/pmsearch';
import { ChangeLocService } from '../../services/changloc.service';

@Component({
  selector: "app-change-loc",
  templateUrl: './change-location.component.html',
  styleUrls: ['./change-location.component.scss']
})

export class ChangeLocationComponent {
  changeLocRequest: changeLocRequest = new changeLocRequest();
  changeLocProjDetails: changeLocProjDetails = new changeLocProjDetails();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    width: "100%",
    type: "default",

  };
  locViewGridList: changeLocSearchResult[] = [];
  changeLocSearchResult: changeLocSearchResult[] = [];
  changeLocDetailsResult: changeLocSearchResult[] = [];
  selectedProject: any;
  selectedView: any;
  selectedProductId: string = '';
  statusList = [];
  locationList = [];
  selectedProjectName = '';
  selectedWarehouse = '';
  selectedLocation = '';

  tableRight: Array<Task> = [];
  tableLeft: Array<Task> = [
    { wareHouse: "house 1", location: "England", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 2", location: "France", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 3", location: "America", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 4", location: "Russia", serialNo: "11", tagNo: "tag-66", qty: "111" },
    { wareHouse: "house 5", location: "Pakistan", serialNo: "11", tagNo: "tag-66", qty: "111" },
  ];

  constructor(private changeLocService: ChangeLocService) {
    this.onAdd = this.onAdd.bind(this);
  }

  onFormSubmit(e: any) {
    console.log(this.changeLocRequest);
    this.search();
    e.preventDefault();
  }
  onSelectionChanged(e: any) {
    console.log(e);
    this.selectedProject = e.addedItems[0];
    console.log(this.selectedProject);
    if (!!this.selectedProject) {
      this.selectedProductId = this.selectedProject.project;
      this.changeLocProjDetails.projectName = this.selectedProject.name;
      this.changeLocProjDetails.location = '';
      this.changeLocProjDetails.warehouse = '';

      this.changeLocService.getChangeLocView(this.selectedProject).subscribe(r => {
        console.log(r);
        this.locViewGridList = r;
        if (r.length !== 0) {
          this.selectedView = r[0];
          this.getDetails();
        }
      });
    }
  }

  onSelectionChangedView(e: any) {
    console.log(e);
    this.selectedView = e.selectedRowsData[0];
    console.log(this.selectedView);
    this.getDetails();
  }

  getDetails() {
    this.changeLocService.getChangeLocItemsDetailsView(this.selectedView).subscribe(r => {
      console.log(r);
      this.changeLocDetailsResult = r;
    });
  }

  search() {
    this.changeLocService.getChangeLocSearchResults(this.changeLocRequest).subscribe(r => {
      console.log(r);
      this.changeLocSearchResult = r;
    });
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
      if (this.tableLeft[event.toIndex] !== undefined) {
        this.tableLeft.splice(event.toIndex, 0, rowData);
      }
      else {
        this.tableLeft.push(rowData);
      }
      this.tableRight.splice(event.fromIndex, 1);
    }
    else if (element == 'column2') {
      if (this.tableRight[event.toIndex] !== undefined) {
        this.tableRight.splice(event.toIndex, 0, rowData);
      }
      else {
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