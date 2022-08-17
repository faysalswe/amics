import { ThisReceiver } from '@angular/compiler';
import { AfterViewInit, Component, OnInit, Output, ViewChild } from '@angular/core';
import 'devextreme/data/odata/store';
import notify from 'devextreme/ui/notify';
import { EventEmitter } from '@angular/core';
import { ChangeLocProjDetails, ChangeLocRequest, ChangeLocSearchResult, ChgLocTransItem } from '../../models/changeLoc';
import { taskItemSearchResult } from '../../models/pmsearch';
import { Warehouse, WarehouseLocation } from '../../models/warehouse';
import { ChangeLocService } from '../../services/changloc.service';
import { SearchService } from '../../services/search.service';
import { LabelMap } from "src/app/pages/models/Label";
import { TextboxStyle } from '../textbox-style/textbox-style';
import { DxDataGridModule,DxDataGridComponent, DxSelectBoxComponent } from 'devextreme-angular';
import { DxiDataGridColumn } from 'devextreme-angular/ui/nested/base/data-grid-column-dxi';
import CheckBox from 'devextreme/ui/check_box'; 

@Component({
  selector: "app-change-loc",
  templateUrl: './change-location.component.html',
  styleUrls: ['./change-location.component.scss']
})

export class ChangeLocationComponent {  
  @ViewChild('locViewGrid', { static: false })  locViewGrid!: DxDataGridComponent;
  @ViewChild('chglocGridVar', { static: false })  chglocGridVar!: DxDataGridComponent;
  @ViewChild('chglocRighttblGridVar', { static: false })  chglocRighttblGridVar!: DxDataGridComponent;  
  //@ViewChild(DxDataGridComponent, { static: false }) locViewGrid!: DxDataGridComponent;    
  //@ViewChild(DxDataGridComponent, { static: false }) chglocGridVar!: DxDataGridComponent;  
  @ViewChild('warehouseVar', { static: false }) warehouseVar!: DxSelectBoxComponent;
  @ViewChild('locationVar', { static: false }) locationVar!: DxSelectBoxComponent;
  @Output() exit : EventEmitter<any> =new EventEmitter<any>();
  
//   setCellValue(newData, value) {
//     let column = (<any>this);
//     column.defaultSetCellValue(newData, value);
// }
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
  warehouseNames: string[] = [];
  validLocationNames: string[] = [];
  selectedProjectName = '';
  selectedWarehouse = '';
  selectedLocation = '';
  groupedWarehouses: any;
  groupedLocations: any;
  pickQty :  number =0;
  
  warehouses: Warehouse[] = [];
  

  locations: WarehouseLocation[] = [];
  //tableRight: Array<ChangeLocSearchResult> = [];
  tableRight: ChangeLocSearchResult[] = [];

  chgLocTransItems: ChgLocTransItem[] = [];
  chgLocTransItemsAdded: ChgLocTransItem[] = [];

  leftSelectedItemKeys: any[] = [];
  rightSelectedItemKeys: any[] = [];

  labelMap: typeof LabelMap;
  StylingMode : string = TextboxStyle.StylingMode;
  LabelMode : string =  TextboxStyle.LabelMode;
 

  constructor(private searchService: SearchService, private changeLocService: ChangeLocService) {
    this.labelMap = LabelMap;
    //this.onAdd = this.onAdd.bind(this);

    this.searchService.getWarehouseInfo('').subscribe(w => {
      this.warehouses = w;
      this.warehouseNames = w.map(w => w.warehouse);
      this.groupedWarehouses = this.groupByKey(w, 'warehouse');
    });

    this.searchService.getLocationInfo('', '').subscribe(l => {
      this.locations = l;
      this.groupedLocations = this.groupByKey(l, 'warehouseId');
      this.validLocationNames = this.locations.map(l => l.location);
      console.log(this.groupedLocations);
      //   console.log(this.groupedLocations['f062f282-ad8e-4743-b01f-2fb9c7ba9f7d']);
    });
    this.changeLocService.DeleteInvTransLoc("admin").subscribe(r => { console.log(r) });

    this.leftSelectionChanged = this.leftSelectionChanged.bind(this);
    this.rightSelectionChanged = this.rightSelectionChanged.bind(this);
    
    // this.setCellValue = this.setCellValue.bind(this);
    // setCellValue(newData, value) {
    //   let column = (<any>this);
    //   column.defaultSetCellValue(newData, value);
  //}  [setCellValue]="setCellValue"
  }
  
  groupByKey(array: any, key: any) {
    return array
      .reduce((hash: any, obj: any) => {
        if (obj[key] === undefined) return hash;
        return Object.assign(hash, { [obj[key]]: (hash[obj[key]] || []).concat(obj) })
      }, {})
  }
  updateWarehouseSelection(location: string = '', onload: boolean = false) {

   // alert(this.warehouse  + " wh " + this.changeLocProjDetails.warehouse );
    if (!this.changeLocProjDetails.warehouse || !location) {
      this.validLocationNames = [];
      this.changeLocProjDetails.location = '';
      return;
    }
    this.changeLocProjDetails.location = '';

    let wid = this.groupedWarehouses[this.changeLocProjDetails.warehouse];
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
    console.log("selectedproj" + this.selectedProject);
    if (!!this.selectedProject) {
      this.selectedProductId = this.selectedProject.project;
      this.changeLocProjDetails.projectName = this.selectedProject.name;
      // this.changeLocProjDetails.warehouse = this.selectedProject.warehouse;
      // this.changeLocProjDetails.location = this.selectedProject.location;
      this.location = '';
      this.warehouse = '';

      this.changeLocService.getChangeLocView(this.selectedProject).subscribe(r => {
        console.log(r);
        this.locViewGridList = r;
        if (r.length !== 0) {
          this.selectedView = r[0];
          this.selectedInvType = !!this.selectedView.invType? this.selectedView.invType : "BASIC" ;
          this.getDetails();
        }
      });
    }
  }

  onSelectionChangedView(e: any) {
    console.log(e);
    this.selectedView = e.selectedRowsData[0];
    this.selectedInvType =!!this.selectedView.invType? this.selectedView.invType : "BASIC" ;
    console.log(this.selectedView);
    this.getDetails();
  }

  getDetails() {
   // this.changeLocService.DeleteInvTransLoc("admin").subscribe(r => { console.log(r) });
    this.changeLocService.getChangeLocItemsDetailsView(this.selectedView).subscribe(r => {
      console.log(r);
      this.changeLocDetailsResult = [];
      if (this.selectedInvType === "BASIC") {
        for (let i = 0; i < r.length; i++) {
          let d = r[i];
          // //d.pickQty = d.quantity;
          d.pickQty = d.pickQty;           
          console.log(d.pickQty);
          this.changeLocDetailsResult.push(d);
          console.log(this.changeLocDetailsResult);                    
        }
        this.checkboxMode="none";
        this.changeLocDetailsResultCopy = [...this.changeLocDetailsResult];
      }
      else {
        this.checkboxMode="always";
        this.changeLocDetailsResult = r;
        this.changeLocDetailsResultCopy = [...r];
      }

      this.tableRight = [];
      this.changeLocService.getChgLocTransLocSelectedItemDetails(this.selectedView.itemsId,'admin',this.selectedView.soLinesId,this.selectedView.invType).subscribe(r => {
        console.log(r);
        this.tableRight = r;      
      });

      let chgLocRows = this.chglocGridVar.instance.getVisibleRows();        

      for (let _i = 0; _i < chgLocRows.length; _i++) {          
        if (chgLocRows[_i].data.pickQty !== undefined) {               
          console.log("pick  " + chgLocRows[_i].data.pickQty + " i val " +_i );               
          this.chglocGridVar.instance.cellValue(_i, 3, '');
        }          
      } 
    });
  }

  search() {   
    let changeLocReq = new ChangeLocRequest();
    changeLocReq.projectId = this.changeLocRequest.projectId;
    changeLocReq.projectName = this.changeLocRequest.projectName;
    changeLocReq.er = this.changeLocRequest.er;
    changeLocReq.budget = this.changeLocRequest.budget;
    changeLocReq.user = '';

   // this.changeLocService.getChangeLocSearchResults(this.changeLocRequest).subscribe(r => {
    this.changeLocService.getChangeLocSearchResults(changeLocReq).subscribe(r => {
      console.log(r);
      this.changeLocSearchResult = r;
    });
  }

  openWarehouseCodeBox() {
    this.warehouseVar?.instance.open();
  }

  openLocationCodeBox() {
  this.locationVar?.instance.open();
  }

  transfer() {
    if (this.changeLocProjDetails.warehouse === "" || this.changeLocProjDetails.location === "") {
      notify({ message: "warehouse and location are required", shading: true, position: top }, "error", 1500);

    } else {
      if (this.tableRight.length !== 0) {
        this.changeLocService.UpdateChangeLoc("admin", this.changeLocProjDetails.warehouse, this.changeLocProjDetails.location).subscribe(r => { this.getDetails(); });
       
      }
      alert("Successfully Transfered");
      this.changeLocService.DeleteInvTransLoc("admin").subscribe(r => { console.log(r) });
      setTimeout(() => {
        this.getDetails();   
       }, 500);
    }
  }

  updateTempTransactionTable() {
    
    if (this.chgLocTransItems.length > 0) {      
      this.changeLocService.UpdateInvTransLoc(this.chgLocTransItems).subscribe((res: any) => {
        this.refreshDataGrid();

        setTimeout(() => {         
          this.getDetails();   
         }, 100);
        
      }, err => { notify({ message: "Error occured during transfer", shading: true, position: top }, "error", 1500) });
      this.chgLocTransItems = [];
    }
  }

  refreshDataGrid() {
    this.chglocGridVar.instance.refresh();
  }

  checkboxMode: string='none';

  onEditorPreparing(e: any) {
    
// if(e.command === "select"){
  
//   if(this.selectedInvType === "BASIC"){
//     e.editorOptions.visible = false; 
//     this.checkboxMode="none";
//     console.log(this.checkboxMode);
//   }
//   else{
//     e.editorOptions.visible = true;  
//     this.checkboxMode="always";
//     console.log(this.checkboxMode);
//   }
// }
 
       if (e.dataField === 'pickQty' && e.parentType === 'dataRow') {
        const defaultValueChangeHandler = e.editorOptions.onValueChanged;
  
        e.editorOptions.onValueChanged = function (this: any, args: any) {
            console.log(args.value);
       
        let leftGridRws = this.chglocGridVar.instance.getVisibleRows();
        leftGridRws.filter((obj:any)=>obj.data.pickQty);        
        let  pickQtyLen = leftGridRws.filter((obj:any)=>obj.data.pickQty === args.value).length;

          if (Number(args.value) > Number(leftGridRws[e.row.rowIndex].data.quantity))          
          {
            alert("Invalid Pick Qty");
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex,
              3,
              '');                
              setTimeout(() => {
                this.chglocGridVar.instance.focus(this.chglocGridVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
              }, 300);         
          }
          else if (Number(args.value) < 0)  {
            alert("Invalid Pick Qty");
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex, 3, '');                
              setTimeout(() => {
                this.chglocGridVar.instance.focus(this.chglocGridVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
              }, 300); 
          }
          else if (Number(args.value) < 0)  {
            alert("Invalid Pick Qty");
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex, 3, '');                
              setTimeout(() => {
                this.chglocGridVar.instance.focus(this.chglocGridVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
              }, 300); 
          }
          else if (!Number(args.value))  {
            alert("Invalid Pick Qty");
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex, 3, '');                
              setTimeout(() => {
                this.chglocGridVar.instance.focus(this.chglocGridVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
              }, 300); 
          }
          else if (args.value.length === 0) {
            alert("Enter Pick Qty");
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex, 3, '');                
              setTimeout(() => {
                this.chglocGridVar.instance.focus(this.chglocGridVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
              }, 300); 
          }
          else{                  
            this.chglocGridVar.instance.cellValue(
              e.row.rowIndex,
              3,
              Number(args.value)
            );            
          }
        }.bind(this);
      }  
    }

  
  onFocusedRowChanged(e: any) 
  {      
    console.log(e.row.data);
    const rowData = e.row && e.row.data;   

    if (rowData) {
      this.pickQty = rowData.pickQty;     
    }
  }

  moveToRight() {
  
    let action = 1;
    let chgLoc_VisibleRows: ChangeLocSearchResult[] = [];
    let pickQtyVar=[];
    if (this.selectedView.invType === 'BASIC'){
        let chgLocRows = this.chglocGridVar.instance.getVisibleRows();
              
        let rowIndex = chgLocRows.find(obj=>obj.data.pickQty === undefined)?.rowIndex;  
                
        for (let _i = 0; _i < chgLocRows.length; _i++) {                  
          if (chgLocRows[_i].data.pickQty !== undefined) {   
            chgLoc_VisibleRows.push(chgLocRows[_i].data);
          }  
          console.log("pickvar " + chgLocRows[_i].data.pickQty);
          pickQtyVar.push(chgLocRows[_i].data.pickQty);
        }   
              
        let picklen=0;
        for (let _i = 0; _i < pickQtyVar.length; _i++){          
          if (pickQtyVar[_i] === undefined) { 
            picklen += 1;
          }
        }

        console.log("pickvar len" + pickQtyVar.length + " picklen " + picklen);
      
        if (picklen == pickQtyVar.length){
          alert("Please enter PickQty");          
          this.chglocGridVar.instance.getCellElement(0, 3);
          return false;
        }
    
      
        if (chgLoc_VisibleRows.length > 0)
          this.changeLocDetailsResult = chgLoc_VisibleRows;
          
        console.log("basic  len" + this.changeLocDetailsResult.length);      
        
      if (this.changeLocDetailsResult.length > 0){
        for (let _i = 0; _i < this.changeLocDetailsResult.length; _i++) {      
          
            // if (this.changeLocDetailsResult[_i].pickQty !== undefined) {  
              
                let transItem = new ChgLocTransItem();
                transItem.action = action;
                
                if (this.selectedView.invType === 'BASIC'){
                  transItem.availQuantity = Number(this.changeLocDetailsResult[_i].quantity);
                  if (Number(this.changeLocDetailsResult[_i].pickQty) !== undefined){
                    transItem.transQuantity = Number(this.changeLocDetailsResult[_i].pickQty);                 
                  }
                  else{
                    alert("Please enter PickQty");
                    return;
                  }
                }
                else{
                  transItem.transQuantity = 1;                
                  transItem.availQuantity = 1;
                }
                  //transItem.availQuantity = Number(this.changeLocDetailsResult[_i].quantity);
                  //transItem.transQuantity = Number(this.changeLocDetailsResult[_i].quantity);

                transItem.soLinesId = this.selectedView.soLinesId;
                transItem.invBasicId = this.changeLocDetailsResult[_i].invBasicId;
                transItem.invSerialId = this.changeLocDetailsResult[_i].invSerialId;
                transItem.id = "";
                transItem.createdBy = 'admin';      
                console.log("qty " +this.changeLocDetailsResult[_i].quantity + "basid " + this.changeLocDetailsResult[_i].invBasicId + " serid " +this.changeLocDetailsResult[_i].invSerialId);        
                this.chgLocTransItems.push(transItem); 

                //this.changeLocDetailsResult[_i].pickQty=0;
            //  }
        
        }  
      }   
    }
    else {
      console.log(this.leftSelectedItemKeys.length );
      if (this.leftSelectedItemKeys.length > 0){
        for (let _i = 0; _i < this.leftSelectedItemKeys.length; _i++) {      
           
             // if (this.changeLocDetailsResult[_i].pickQty !== undefined) {  
               
                let transItem = new ChgLocTransItem();
                transItem.action = action;
                transItem.transQuantity = 1;                
                transItem.availQuantity = 1;
                transItem.soLinesId = this.selectedView.soLinesId;
                transItem.invBasicId = this.leftSelectedItemKeys[_i].invBasicId;
                transItem.invSerialId = this.leftSelectedItemKeys[_i].invSerialId;
                transItem.id = "";
                transItem.createdBy = 'admin';                    
                this.chgLocTransItems.push(transItem);   
        }  
      }
    }    
    this.updateTempTransactionTable();    
  }

  moveToLeft() {    
    let action = 2;   

    let chgLocRightTblRows = this.chglocRighttblGridVar.instance.getVisibleRows();
    
    for (let i = 0; i < this.rightSelectedItemKeys.length; i++) {
      var index = this.tableRight.findIndex(d => d.invBasicId === this.rightSelectedItemKeys[i].invBasicId && d.invSerialId === this.rightSelectedItemKeys[i].invSerialId);
      let rowData = new ChangeLocSearchResult();
      let itemData = this.changeLocDetailsResultCopy.filter(d => d.invBasicId === this.rightSelectedItemKeys[i].invBasicId && d.invSerialId === this.rightSelectedItemKeys[i].invSerialId)[0];
            
      let transItem = new ChgLocTransItem();
      transItem.action = action;
      if (this.rightSelectedItemKeys[i].invSerialId !== "")
         transItem.availQuantity = Number("1");
      else
        transItem.availQuantity = Number(this.rightSelectedItemKeys[i].quantity);
      
    //  transItem.transQuantity = Number(rowData.quantity);
      transItem.soLinesId = this.selectedView.soLinesId;
      transItem.invBasicId = this.rightSelectedItemKeys[i].invBasicId;
      transItem.invSerialId =  this.rightSelectedItemKeys[i].invSerialId;
      transItem.id = "";
      transItem.createdBy = "admin";
      this.chgLocTransItems.push(transItem);
    }
    console.log("move to left");
    this.updateTempTransactionTable();

  }

  onCellPrepared(e: any)
  {
    console.log("cell prepared");
    if (this.selectedInvType != "SERIAL") {  
      console.log("cell prepared SERIAL");
      var editor = CheckBox.getInstance(e.cellElement.querySelector(".dx-select-checkbox"));  
      editor.option("disabled", true);  
      e.cellElement.style.pointerEvents = 'none';  
  }  
  }

  leftSelectionChanged(data: any) {
    console.log("leftSelectionChanged" + data.selectedRowKeys);
   // console.log(data)    
    //if (this.selectedInvType  !== "BASIC")
      this.leftSelectedItemKeys = data.selectedRowKeys;
  }

  rightSelectionChanged(data: any) {
    console.log("rightSelectionChanged");
    console.log(data)
    console.log(data.selectedRowKeys);
    this.rightSelectedItemKeys = data.selectedRowKeys;
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