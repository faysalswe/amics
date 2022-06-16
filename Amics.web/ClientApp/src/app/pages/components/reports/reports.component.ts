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
  partnumber:string[]=[];
  itemClassList: ItemClass[] = [];
  partnumberList:PartNumber[] = [];
  itemtypeList:ItemType[] = [];
  itemcodeList:ItemCode[] = [];
  warehouseList:WareHouse[] = [];
  locationList:ReportLocation[] = [];
  defaultpartnumber:string='';
  

  tabsData = [
    {Name : "Inventory", items: [
      {ID:1,"reportNo": "101", "reportName": "item list", "description": "List of all item in inventory"},
      {ID:2,"reportNo": "104", "reportName": "Inventory", "description": "abcddddddd"},
      {ID:3,"reportNo": "106", "reportName": "Multiple Inventory Location", "description": "location desc"},
      {ID:4,"reportNo": "107", "reportName": "item list", "description": "List of all item in inventory"},
      {ID:5,"reportNo": "108", "reportName": "Inventory", "description": "abcddddddd"},
      {ID:6,"reportNo": "109", "reportName": "Multiple Inventory Location", "description": "location desc"},
    
    ]},
    {Name : "Sales Order", items: [
      {ID:1,"reportNo": "201", "reportName": "sale list", "description": "List of all item in Sales"},
      {ID:1,"reportNo": "204", "reportName": "Sale Order", "description": "abcddddddd"},
      {ID:1,"reportNo": "206", "reportName": "Multiple Sale Location", "description": "Sale location desc"},
      {ID:1,"reportNo": "206", "reportName": "Multiple Sale Location", "description": "Sale location desc"},
    ]},
    {Name : "Purchase Order", items: [
      {ID:1,"reportNo": "301", "reportName": "Purchase list",              "description": "List of all item in Purchase"},
      {ID:1,"reportNo": "304", "reportName": "Purchase",                    "description": "Purchase"},
      {ID:1,"reportNo": "406", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {ID:1,"reportNo": "406", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {ID:1,"reportNo": "406", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
      {ID:1,"reportNo": "406", "reportName": "Multiple Purchase Location", "description": "Purchase location desc"},
    ]}
  ];


  constructor() { }

  ngOnInit(): void {
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

  showModel(reportNo: string){
    if(reportNo == "101" || reportNo == "104"){
      this.popupVisible_101_104 = true;
    }
    else if(reportNo == "106"){
      this.popupVisible_106 = true;
    }
  }
}
