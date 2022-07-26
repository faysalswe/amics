import { Component, OnInit } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import { BulkTransferService } from '../../services/bulktransfer.service';
import { BulkTransferItem,BulkTransferItemResult,UpdateBulkTransfer } from "../../models/bulktransfer";
import { InventoryService } from '../../services/inventory.service';
import { TransLogInt } from 'src/app/shared/models/rest.api.interface.model';
import { LabelMap } from '../../models/Label';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { AfterViewInit,  ViewChild } from '@angular/core';
import {
  DxFormComponent,
  DxSelectBoxComponent,
  DxTextBoxComponent,
} from 'devextreme-angular';

@Component({
  selector: 'app-bulk-transfer',
  templateUrl: './bulk-transfer.component.html',
  styleUrls: ['./bulk-transfer.component.scss']
})
export class BulkTransferComponent {
 // @ViewChild('varwarehouse', { static: false })
  varWarehouse!: DxSelectBoxComponent;

  fromWarehouse: string = '';
  fromLocation: string = '';
  
  toWarehouse: string = '';
  toLocation: string = '';
  bulkGridList: BulkTransferItemResult[] = [];  
  
  fromWarehouseList: string[] = [];
  fromWarehouses: Warehouse[] = [];
  groupedWarehouses: any;

  fromLocations: WarehouseLocation[] = [];
  groupedLocations: any;  
  fromLocationList : string[] = [];

  toWarehouseList: string[] = [];
  toWarehouses: Warehouse[] = []; 

  toLocations: WarehouseLocation[] = [];  
  toLocationList : string[] = [];  
  
  warehouse: string = '';
  location: string = '';
  validLocId: string[] = [];
  updBulkTransferLst: UpdateBulkTransfer[] = [];
  
  loadingVisible = false;

  fromDate: Date = new Date();
  toDate: Date = new Date();
  trasLogArray: TransLogInt[] = [];
  labelMap: typeof LabelMap;
  gridrowlen= 0;

  ngOnInit(): void {
    this.loadingVisible = true;    
    this.fromDate.setMonth(this.fromDate.getMonth() - 1);
  }
 
 constructor(private searchService: SearchService, private bulktransService : BulkTransferService,private inventoryService: InventoryService) {
  //this.fromWarehouse.focus();

  this.searchService.getWarehouseInfo('').subscribe(w => {
    this.fromWarehouses = w; //contains id, warehouses  
    console.log(this.fromWarehouse);       
    this.fromWarehouseList =  w.map(w => w.warehouse);   //only warehouses
    this.groupedWarehouses = this.groupByKey(w, 'warehouse');    
  });
  
  this.searchService.getLocationInfo('', '').subscribe(l => {
    this.fromLocations = l;
    this.groupedLocations = this.groupByKey(l, 'warehouseId');    
  });

  //================= To Warehouse & Location ===========================
  this.searchService.getWarehouseInfo('').subscribe(w => {
    this.toWarehouses = w; 
    console.log("to ware " + this.toWarehouse);
    this.toWarehouseList =  w.map(w => w.warehouse);   //only warehouses
    this.groupedWarehouses = this.groupByKey(w, 'warehouse');    
  });
  
  this.searchService.getLocationInfo('', '').subscribe(l => {
    this.toLocations = l;
    this.groupedLocations = this.groupByKey(l, 'warehouseId');    
  });
  
  this.labelMap = LabelMap;
  this.refreshLog();
  //=============================== End ===================================
}

groupByKey(array: any, key: any) {
  return array
    .reduce((hash: any, obj: any) => {
      if (obj[key] === undefined) return hash;
      return Object.assign(hash, { [obj[key]]: (hash[obj[key]] || []).concat(obj) })
    }, {})
}

openFmWarehouseBox() {
  this.varWarehouse?.instance.focus();  
}

updateWarehouseSelection(location: string = '', onload: boolean = false) {
  
  if (!this.fromWarehouse || !location) {
      this.fromLocationList = [];
      this.location = '';
      return;
  }  
  this.fromLocation='';
    
  let wid = this.groupedWarehouses[this.fromWarehouse];
  console.log(wid[0].id);

  if (!!wid) {
      let fromLocations: WarehouseLocation[] = this.groupedLocations[wid[0].id];
      this.fromLocationList = fromLocations.map(l => l.location);      
  } else {     
    this.fromLocationList = []; 
  }
}

onSelectValidateLoc(e: any) {
  console.log("On select changed loc");
  this.bulktransService.validateBulkTransferItem(this.fromWarehouse, this.fromLocation).subscribe(resLocId => {
    console.log("validateBulkTransferItem");
    console.log(resLocId);
     this.validLocId = resLocId;    
  });

  if (this.validLocId[0] !== ""){
      this.bulktransService.getBulkTransferItemDetails(this.fromWarehouse, this.fromLocation).subscribe(r => {  
      console.log(r);
      this.gridrowlen = r.length;
      //this.bulkTransferItemResultset = r;
      this.bulkGridList=r;
    });
  }
  else
  {
    console.log("Please select a valid location");
  }
}

updateToWarehouseSelection(location: string = '', onload: boolean = false) {
  
  if (!this.toWarehouse || !location) {
      this.toLocationList = [];
      this.location = '';
      return;
  }  
  this.toLocation='';

  let wid = this.groupedWarehouses[this.toWarehouse];  

  if (!!wid) {    
      let toLocations: WarehouseLocation[] = this.groupedLocations[wid[0].id];
      this.toLocationList = toLocations.map(l => l.location);      
  } else {     
    this.toLocationList = []; 
  }
}

goClick()
{  
  this.bulktransService.validateBulkTransferItem(this.fromWarehouse, this.fromLocation).subscribe(resLocId => {
     this.validLocId = resLocId;         
     console.log(resLocId);
  });
 
  if (this.validLocId[0] !== ""){
      this.bulktransService.getBulkTransferItemDetails(this.fromWarehouse, this.fromLocation).subscribe(r => {  
      console.log(r);      
      this.gridrowlen = r.length;
      //this.bulkTransferItemResultset = r;
      this.bulkGridList=r;
    });
  }
  else
  {
    console.log("Please select a valid location");
  }
}

transferBtnClick()
{
  console.log("tranfer to click");
  let updTransItem = new UpdateBulkTransfer();
  updTransItem.WarehouseFrom = this.fromWarehouse;
  updTransItem.LocationFrom = this.fromLocation;
  updTransItem.WarehouseTo = this.toWarehouse;
  updTransItem.LocationTo = this.toLocation;
  updTransItem.UserName = 'admin';
 // this.updBulkTransferLst.push(updTransItem);
  if (!!this.fromWarehouse && !!this.fromLocation && !!this.toWarehouse && !!this.toLocation){
    if ((this.fromWarehouse === this.toWarehouse) && (this.fromLocation === this.toLocation))
    {
      alert("'From Warehouse & Location' & 'To Warehouse & Location' must not be same");  
      return; 
    }
    else{
      this.bulktransService.executeBulkTransferItemDetails(updTransItem).subscribe(r => {
        console.log(r);      
      });
    }
  }
  else{
    alert("Please select Warehouse and Location");    
    return;
  }
}

fromDateStr(): string {
  return (
    this.fromDate.getFullYear() +
    '-' +
    (this.fromDate.getMonth() + 1) +
    '-' +
    this.fromDate.getDate()
  );
}

toDateStr(): string {
  return (
    this.toDate.getFullYear() +
    '-' +
    (this.toDate.getMonth() + 1) +
    '-' +
    this.toDate.getDate()
  );
}

fromDatehandler(e: any) {
  const previousValue = e.previousValue;
  const newValue = e.value;
  this.fromDate = e.value;
}

toDatehandler(e: any) {
  const previousValue = e.previousValue;
  const newValue = e.value;
  this.toDate = e.value;
}

refreshLog() {  
  this.loadingVisible = true;
  this.inventoryService
    .getTransLog(this.fromDateStr(), this.toDateStr())
    .subscribe((obj: TransLogInt[]) => {
      this.trasLogArray = obj;
      console.log(obj.length);
      this.loadingVisible = false;
    });
}
};