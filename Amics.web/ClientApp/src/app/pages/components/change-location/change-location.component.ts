import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit, Output } from '@angular/core';
import 'devextreme/data/odata/store';
import notify from 'devextreme/ui/notify';
import { EventEmitter } from '@angular/core';
import { ChangeLocProjDetails, ChangeLocRequest, ChangeLocSearchResult, ChgLocTransItem } from '../../models/changeLoc';
import { taskItemSearchResult } from '../../models/pmsearch';
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import { ChangeLocService } from '../../services/changloc.service';
import { SearchService } from '../../services/search.service';

@Component({
  selector: "app-change-loc",
  templateUrl: './change-location.component.html',
  styleUrls: ['./change-location.component.scss']
})

export class ChangeLocationComponent {

  @Output() exit : EventEmitter<any> =new EventEmitter<any>();

  warehouse: string = '';
  location: string = '';
  changeLocRequest: ChangeLocRequest = new ChangeLocRequest();
  changeLocProjDetails: ChangeLocProjDetails = new ChangeLocProjDetails();
  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    width: "100%",
    type: "default",

  };
  locViewGridList: ChangeLocSearchResult[] = [];
  changeLocSearchResult: ChangeLocSearchResult[] = [];
  changeLocDetailsResult: ChangeLocSearchResult[] = [];
  selectedProject: any;
  selectedView: any;
  selectedProductId: string = '';
  statusList = [];
  locationList = [];
  warehouseNames: string[] = [];
  validLocationNames: string[] = [];
  selectedProjectName = '';
  selectedWarehouse = '';
  selectedLocation = '';
  groupedWarehouses: any;
  groupedLocations: any;

  
  warehouses: Warehouse[] = [];
  

  locations: WarehouseLocation[] = [];
  tableRight: Array<ChangeLocSearchResult> = [];
  chgLocTransItems: ChgLocTransItem[] = [];
  chgLocTransItemsAdded: ChgLocTransItem[] = [];

  constructor(private searchService: SearchService, private changeLocService: ChangeLocService) {
    this.onAdd = this.onAdd.bind(this);
    this.searchService.getWarehouseInfo('').subscribe(w => {
      this.warehouses = w;
      this.warehouseNames = w.map(w => w.warehouse);
      this.groupedWarehouses = this.groupByKey(w, 'warehouse');
    });

    this.searchService.getLocationInfo('', '').subscribe(l => {
      this.locations = l;
      this.groupedLocations = this.groupByKey(l, 'warehouseId');
      console.log(this.groupedLocations);
      //   console.log(this.groupedLocations['f062f282-ad8e-4743-b01f-2fb9c7ba9f7d']);
    });
  }
  groupByKey(array: any, key: any) {
    return array
      .reduce((hash: any, obj: any) => {
        if (obj[key] === undefined) return hash;
        return Object.assign(hash, { [obj[key]]: (hash[obj[key]] || []).concat(obj) })
      }, {})
  }
  updateWarehouseSelection(location: string = '', onload: boolean = false) {

    if (!this.warehouse || !location) {
      this.validLocationNames = [];
      this.location = '';
      return;
    }
    this.location = '';

    let wid = this.groupedWarehouses[this.warehouse];
    if (!!wid) {
      let locations: WarehouseLocation[] = this.groupedLocations[wid[0].id];
      this.validLocationNames = locations.map(l => l.location);
    } else { this.validLocationNames = []; }
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
      this.location = '';
      this.warehouse = '';

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
      this.tableRight = [];
    });
  }

  search() {
    this.changeLocService.getChangeLocSearchResults(this.changeLocRequest).subscribe(r => {
      console.log(r);
      this.changeLocSearchResult = r;
    });
  }


  onAdd(event: any) {
    let rowData = new ChangeLocSearchResult();
    let itemData = event.itemData;
    rowData.warehouse = itemData.warehouse;
    rowData.location = itemData.location;
    rowData.serNo = itemData.serNo;
    rowData.tagNo = itemData.tagNo;
    rowData.quantity = itemData.quantity;

    let element = event.element.parentElement.className;
    let action = 0;
    if (element == 'column1') {
      if (this.changeLocDetailsResult[event.toIndex] !== undefined) {
        this.changeLocDetailsResult.splice(event.toIndex, 0, rowData);
      }
      else {
        this.changeLocDetailsResult.push(rowData);
      }
      this.tableRight.splice(event.fromIndex, 1);
      action = 2;
    }
    else if (element == 'column2') {
      if (this.tableRight[event.toIndex] !== undefined) {
        this.tableRight.splice(event.toIndex, 0, rowData);
      }
      else {
        this.tableRight.push(rowData);
      }
      this.changeLocDetailsResult.splice(event.fromIndex, 1);
      action = 1;
    }
    var transItem = new ChgLocTransItem();
    transItem.action = action;
    transItem.availQuantity = rowData.quantity;
    transItem.transQuantity = rowData.quantity;
    transItem.soLinesId = rowData.soLinesId;
    transItem.invBasicId = rowData.invBasicId;
    transItem.invSerialId = rowData.invSerialId;
    transItem.id = "";
    transItem.createdBy = 'admin';
    this.chgLocTransItems.push(transItem);
    this.changeLocService.UpdateInvTransLoc(this.chgLocTransItems).subscribe(id => {
      if (action == 1) {
        let item = this.tableRight.find(r => r.soLinesId === rowData.soLinesId);
        transItem.id = id;
        if (action == 1) {
          this.chgLocTransItemsAdded.push(transItem);
        }
      }
    }, err => { notify({ message: "Error occured during transfer", shading: true, position: top }, "error", 1500) });
    this.chgLocTransItems = [];
  }

  onExit(){
    this.exit.emit();
  }
}

export class Task {
  wareHouse: string = "";
  location: string = "";
  serialNo: string = "";
  tagNo: string = "";
  qty: string = "";
}