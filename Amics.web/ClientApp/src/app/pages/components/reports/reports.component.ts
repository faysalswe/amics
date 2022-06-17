import { Component, OnInit } from '@angular/core';
import { Guid } from 'guid-typescript';
import { ItemClass, ItemCode, ItemType, PartNumber, ReportLocation, WareHouse } from '../../models/searchModels';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  pivotGridDataSource: any;
  popupVisible_101_104 = false;
  popupVisible_106 = false;
  popupVisible_109 = false;
  popupVisible_118 = false;
  partnumber:string[]=[];
  itemClassList: ItemClass[] = [];
  partnumberList:PartNumber[] = [];
  itemtypeList:ItemType[] = [];
  itemcodeList:ItemCode[] = [];
  warehouseList:WareHouse[] = [];
  locationList:ReportLocation[] = [];


  itemNumberList = [];
  locationTransferList = [];
  defaultpartnumber:string='';
  

  tabsData = [
    {name : "Inventory", items: [
      {id:1,"reportId": "101", "reportName": "item list", "description": "List of all item in inventory"},
      {id:2,"reportId": "104", "reportName": "Inventory", "description": "abcddddddd"},
      {id:3,"reportId": "106", "reportName": "Multiple Inventory Location", "description": "location desc"},
      {id:6,"reportId": "109", "reportName": "Multiple Inventory Location", "description": "location desc"},
      {id:6,"reportId": "118", "reportName": "Multiple Inventory Location", "description": "location desc"},
    
    ]},
    {name : "Sales Order", items: [
      {id:1,"reportId": "201", "reportName": "sale list", "description": "List of all item in Sales"},
      {id:2,"reportId": "204", "reportName": "Sale Order", "description": "abcddddddd"},
      {id:3,"reportId": "206", "reportName": "Multiple Sale Location", "description": "Sale location desc"},
      {id:4,"reportId": "207", "reportName": "Multiple Sale Location", "description": "Sale location desc"},
    ]},
    {name : "Purchase Order", items: [
      {id:1,"reportId": "301", "reportName": "Purchase list",              "description": "List of all item in Purchase"},
      {id:2,"reportId": "304", "reportName": "Purchase",                    "description": "Purchase"},
      {id:3,"reportId": "401", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {id:4,"reportId": "402", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {id:5,"reportId": "403", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {id:6,"reportId": "404", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
    ]}
  ];


  constructor() { }

  ngOnInit(): void {
  }

  onSelectionChanged(e: any) {
    console.log(e);
    var selectedItem = e.selectedRowsData[0];
    console.log(selectedItem);
    this.showModel(selectedItem.reportId)
    
}

  // onClick(Report #) {
  //   that.popupVisible = false;
  // },
  // onPivotCellClick(e: { area: string; cell: { rowPath: string | any[]; }; }) {
  //   if (e.area == 'data') {
  //     const rowPathLength = e.cell.rowPath.length;
  //     const rowPathName = e.cell.rowPath[rowPathLength - 1];

      
    // }
  // }
  partnumberLbl:string = "partnumber";

  showModel(reportId: string){
    if(reportId == "101" || reportId == "104"){
      this.popupVisible_101_104 = true;
    }
    else if(reportId == "106"){
      this.popupVisible_106 = true;
    }
    else if(reportId == "109"){
      this.popupVisible_109 = true;
    }
    else if(reportId == "118"){
      this.popupVisible_118 = true;
    }
  }
}

