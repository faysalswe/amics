import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import 'devextreme/data/odata/store';
import notify from 'devextreme/ui/notify';
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
  changeLocDetailsResultCopy: ChangeLocSearchResult[] = [];
  selectedProject: any;
  selectedView: any;
  selectedInvType = "BASIC";

  selectedProductId: string = '';
  statusList = [];
  locationList = [];
  selectedProjectName = '';
  selectedWarehouse = '';
  selectedLocation = '';

  warehouseNames: string[] = [];
  warehouses: Warehouse[] = [];
  groupedWarehouses: any;

  locations: WarehouseLocation[] = [];
  groupedLocations: any;
  validLocationNames: string[] = [];
  tableRight: Array<ChangeLocSearchResult> = [];
  chgLocTransItems: ChgLocTransItem[] = [];
  chgLocTransItemsAdded: ChgLocTransItem[] = [];

  leftSelectedItemKeys: any[] = [];
  rightSelectedItemKeys: any[] = [];

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

    this.leftSelectionChanged = this.leftSelectionChanged.bind(this);
    this.rightSelectionChanged = this.rightSelectionChanged.bind(this);
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
          this.selectedInvType = this.selectedView.invType;
          this.getDetails();
        }
      });
    }
  }

  onSelectionChangedView(e: any) {
    console.log(e);
    this.selectedView = e.selectedRowsData[0];
    this.selectedInvType = this.selectedView.invType;
    console.log(this.selectedView);
    this.getDetails();
  }

  getDetails() {
    this.changeLocService.DeleteInvTransLoc("admin").subscribe(r => { console.log(r) });
    this.changeLocService.getChangeLocItemsDetailsView(this.selectedView).subscribe(r => {
      console.log(r);
      this.changeLocDetailsResult = [];
      if (this.selectedInvType === "BASIC") {
        for (let i = 0; i < r.length; i++) {
          let d = r[i];
          d.pickQty = d.quantity;
          this.changeLocDetailsResult.push(d);
        }
        this.changeLocDetailsResultCopy = [...this.changeLocDetailsResult];
      }
      else {
        this.changeLocDetailsResult = r;
        this.changeLocDetailsResultCopy = [...r];
      }

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
    rowData.invBasicId = itemData.invBasicId;
    rowData.invSerialId = itemData.invSerialId;
    rowData.invType = itemData.invType;
    rowData.itemsId = itemData.itemsId;
    rowData.pickQty = this.selectedInvType === 'SERIAL' ? itemData.quantity : itemData.pickQty;

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
    transItem.availQuantity = Number(rowData.quantity);
    transItem.transQuantity = Number(rowData.pickQty);
    transItem.soLinesId = this.selectedView.soLinesId;
    transItem.invBasicId = rowData.invBasicId;
    transItem.invSerialId = rowData.invSerialId;
    transItem.id = "";
    transItem.createdBy = 'admin';
    this.chgLocTransItems.push(transItem);

    if (action == 1) {
      if (rowData.quantity !== rowData.pickQty) {
        rowData.quantity = rowData.quantity - rowData.pickQty;
        rowData.pickQty = rowData.quantity;
        this.changeLocDetailsResult.push(rowData);
      }
    }

    this.changeLocService.UpdateInvTransLoc(this.chgLocTransItems).subscribe((res: any) => {
    }, err => { notify({ message: "Error occured during transfer", shading: true, position: top }, "error", 1500) });

    this.chgLocTransItems = [];


  }

  transfer() {
    if (this.warehouse === "" || this.location === "") {
      notify({ message: "warehouse and location are required", shading: true, position: top }, "error", 1500);

    } else {
      if (this.tableRight.length !== 0) {
        this.changeLocService.UpdateChangeLoc("admin", this.warehouse, this.location).subscribe(r => { this.getDetails(); });
      }
      console.log("transfer clicked");
    }
  }

  updateTempTransactionTable() {
    if (this.chgLocTransItems.length > 0) {
      this.changeLocService.UpdateInvTransLoc(this.chgLocTransItems).subscribe((res: any) => {

      }, err => { notify({ message: "Error occured during transfer", shading: true, position: top }, "error", 1500) });
      this.chgLocTransItems = [];

    }
  }

  moveToRight() {
    let action = 1;
    for (let i = 0; i < this.leftSelectedItemKeys.length; i++) {
      var index = this.changeLocDetailsResult.findIndex(d => d.invBasicId === this.leftSelectedItemKeys[i].invBasicId && d.invSerialId === this.leftSelectedItemKeys[i].invSerialId);
      let rowData = new ChangeLocSearchResult();
      let itemData = this.changeLocDetailsResult.filter(d => d.invBasicId === this.leftSelectedItemKeys[i].invBasicId && d.invSerialId === this.leftSelectedItemKeys[i].invSerialId)[0];
      rowData.warehouse = itemData.warehouse;
      rowData.location = itemData.location;
      rowData.serNo = itemData.serNo;
      rowData.tagNo = itemData.tagNo;
      rowData.quantity = itemData.quantity;
      rowData.invBasicId = itemData.invBasicId;
      rowData.invSerialId = itemData.invSerialId;
      rowData.invType = itemData.invType;
      rowData.itemsId = itemData.itemsId;
      rowData.pickQty = this.selectedInvType === 'SERIAL' ? itemData.quantity : itemData.pickQty;

      let transItem = new ChgLocTransItem();
      transItem.action = action;
      transItem.availQuantity = Number(rowData.quantity);
      transItem.transQuantity = Number(rowData.pickQty);
      transItem.soLinesId = this.selectedView.soLinesId;
      transItem.invBasicId = rowData.invBasicId;
      transItem.invSerialId = rowData.invSerialId;
      transItem.id = "";
      transItem.createdBy = 'admin';
      this.chgLocTransItems.push(transItem);

      this.tableRight.push(rowData);
      this.changeLocDetailsResult.splice(index, 1);
      if (rowData.quantity !== rowData.pickQty) {
        rowData.quantity = rowData.quantity - rowData.pickQty;
        rowData.pickQty = rowData.quantity;
        this.changeLocDetailsResult.push(rowData);
      }

    }
    console.log("move to right");
    this.updateTempTransactionTable();

  }


  moveToLeft() {
    let action = 2;
    for (let i = 0; i < this.rightSelectedItemKeys.length; i++) {
      var index = this.tableRight.findIndex(d => d.invBasicId === this.rightSelectedItemKeys[i].invBasicId && d.invSerialId === this.rightSelectedItemKeys[i].invSerialId);
      let rowData = new ChangeLocSearchResult();
      let itemData = this.changeLocDetailsResultCopy.filter(d => d.invBasicId === this.rightSelectedItemKeys[i].invBasicId && d.invSerialId === this.rightSelectedItemKeys[i].invSerialId)[0];
      rowData.warehouse = itemData.warehouse;
      rowData.location = itemData.location;
      rowData.serNo = itemData.serNo;
      rowData.tagNo = itemData.tagNo;
      rowData.quantity = itemData.quantity;
      rowData.pickQty = itemData.quantity;
      rowData.invBasicId = itemData.invBasicId;
      rowData.invSerialId = itemData.invSerialId;
      rowData.invType = itemData.invType;
      rowData.itemsId = itemData.itemsId;
      this.changeLocDetailsResult.push(rowData);
      this.tableRight.splice(index, 1);

      let transItem = new ChgLocTransItem();
      transItem.action = action;
      transItem.availQuantity = Number(rowData.quantity);
      transItem.transQuantity = Number(rowData.quantity);
      transItem.soLinesId = this.selectedView.soLinesId;
      transItem.invBasicId = rowData.invBasicId;
      transItem.invSerialId = rowData.invSerialId;
      transItem.id = "";
      transItem.createdBy = 'admin';
      this.chgLocTransItems.push(transItem);

    }
    console.log("move to left");
    this.updateTempTransactionTable();

  }
  leftSelectionChanged(data: any) {
    this.leftSelectedItemKeys = data.selectedRowKeys;

  }
  rightSelectionChanged(data: any) {
    this.rightSelectedItemKeys = data.selectedRowKeys;
  }
}

export class Task {
  wareHouse: string = "";
  location: string = "";
  serialNo: string = "";
  tagNo: string = "";
  qty: string = "";
}