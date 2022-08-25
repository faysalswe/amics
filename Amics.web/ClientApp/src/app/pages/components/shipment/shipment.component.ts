import { AfterViewInit, Component, OnInit, Output, ViewChild } from '@angular/core';
import 'devextreme/data/odata/store';
import { EventEmitter } from '@angular/core';
import notify from 'devextreme/ui/notify';
import { LabelMap } from '../../models/Label';
import { TextboxStyle } from '../textbox-style/textbox-style';
import { ShipmentSearch, ShipmentSearchResult, ShipmentSelectedItem, ShipmentViewResult, ShipmentPickShipItem } from '../../models/shipment';
import { ColumnExpressionCollectionHelper } from '@devexpress/analytics-core/queryBuilder-internal';
import { DxFormModule } from "devextreme-angular";
import { ShipmentService } from '../../services/shipment.service';
import { AuthService } from 'src/app/shared/services';
import { DxDataGridModule,DxDataGridComponent, DxSelectBoxComponent } from 'devextreme-angular';

@Component({
  selector: 'app-shipment',
  templateUrl: './shipment.component.html',
  styleUrls: ['./shipment.component.scss']
})
export class ShipmentComponent {
  gridList = [];
  @ViewChild('shipmtLftTblVar', { static: false })  shipmtLftTblVar!: DxDataGridComponent;  
  shipmentSearch: ShipmentSearch = new ShipmentSearch();
  shipmtSearchSelected: ShipmentSelectedItem = new ShipmentSelectedItem();
  //shipmentViewResult: ShipmentViewResult = new ShipmentViewResult();
  
  shipmtSearchResult: ShipmentSearchResult[] = [];
  shipViewGridList: ShipmentViewResult[] = [];
  shipViewLftTblDetails: ShipmentViewResult[] = [];
  shipViewRightTblDetails: ShipmentViewResult[] = [];
  shipInvPickShipItem: ShipmentPickShipItem[] = [];
  
  leftSelectedItemKeys: any[] = [];
  rightSelectedItemKeys: any[] = [];
  pickQty :  number =0;  
  checkboxMode: string='none';

 constructor(private shipmtService: ShipmentService, private authService: AuthService) {
  this.labelMap = LabelMap;
  console.log("shipment file " + this.labelMap.contact_num );

  this.shipmtService.DeleteInvPickShip("admin").subscribe(res => {
    console.log(res);
  });
  //this.onAdd = this.onAdd.bind(this);
}

  submitButtonOptions = {
    text: "Search",
    useSubmitBehavior: true,
    width: "100%",
    type: "default"
  };
  
  selectedProject: any;  
  selectedInvType = "BASIC";
  selectedProjectId: string = '';
  selectedRowView: any;

  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap
 
  onFormSubmit(e: any) {
    console.log(" onFormSubmit "  + this.shipmentSearch);
    console.log(this.authService.currentUser);
    this.SearchBtnClick();
    e.preventDefault();
  }

  SearchBtnClick() {   
    console.log("btn search");
    let shipSearch = new ShipmentSearch();
    shipSearch.projectId = this.shipmentSearch.projectId;
    shipSearch.projectName = this.shipmentSearch.projectName;
    shipSearch.er = this.shipmentSearch.er;
    shipSearch.budget = this.shipmentSearch.budget;
    shipSearch.task = this.shipmentSearch.task;;
    console.log(shipSearch.projectId  + " proj name " + shipSearch.projectName + " er "+ shipSearch.er );
    this.shipmtService.getShipmentSearchResult(shipSearch).subscribe(r => {
      console.log(r);
      this.shipmtSearchResult = r;
    });
  }

  onSelectionChanged(e: any){
    this.selectedProject = e.addedItems[0];
    console.log(this.selectedProject);
    if (!!this.selectedProject) {
      this.selectedProjectId = this.selectedProject.project;
      this.shipmtSearchSelected.projectName = this.selectedProject.name; 
      this.shipmtSearchSelected.mdatOut = this.selectedProject.mdatOut;      
      console.log(this.selectedProject.project + " proj somain  "+ this.selectedProject.soMain);
      
    
      this.shipmtService.getShipmentView(this.selectedProject).subscribe(r => {
        console.log(r);
        this.shipViewGridList = r;
        if (r.length !== 0) {
          this.selectedRowView = r[0];
          this.selectedInvType = !!this.selectedRowView.invType? this.selectedRowView.invType : "BASIC" ;
          console.log("invtype " + this.selectedInvType + " type " + this.selectedRowView.invType);
          this.getDetails();
        }
      });
    }
  }

  onSelectionChangedView(e: any) {
    this.selectedRowView = e.selectedRowsData[0];
    this.selectedInvType =!!this.selectedRowView.invType? this.selectedRowView.invType : "BASIC" ;
    console.log(this.selectedRowView);
    console.log(this.selectedRowView.invType);
    this.getDetails();
  }

  getDetails(){
    console.log(this.selectedRowView);    
    this.shipmtService.getShipmentViewDetails(this.selectedRowView).subscribe(r => {
      console.log(r);
      this.shipViewLftTblDetails = r;
     if (this.selectedInvType === "BASIC") {     
      this.checkboxMode="none";      
    }
    else {
      this.checkboxMode="always";      
    }

    });

    this.shipViewRightTblDetails = [];    
      this.shipmtService.getShipmentSelectedItemDetails(this.selectedRowView).subscribe(r => {
        console.log(r);
        this.shipViewRightTblDetails = r;      
     });

     let shipRows = this.shipmtLftTblVar.instance.getVisibleRows();        

      for (let _i = 0; _i < shipRows.length; _i++) {          
        if (shipRows[_i].data.pickQty !== undefined) {               
          console.log("pick  " + shipRows[_i].data.pickQty + " i val " +_i );               
          this.shipmtLftTblVar.instance.cellValue(_i, 3, '');
        }          
      } 
  }
  
  leftTblSelectionChanged(e: any){    
    console.log(e.selectedRowKeys);
    this.leftSelectedItemKeys = e.selectedRowKeys;
  }
  
  rightTblSelectionChanged(e: any){    
    console.log(e.selectedRowKeys);
    this.rightSelectedItemKeys = e.selectedRowKeys;
  }

  onEditorPreparing(e: any) {
     
      if (e.dataField === 'pickQty' && e.parentType === 'dataRow') {
      const defaultValueChangeHandler = e.editorOptions.onValueChanged;

      e.editorOptions.onValueChanged = function (this: any, args: any) {
          console.log(args.value);
      
      let leftGridRowData = this.shipmtLftTblVar.instance.getVisibleRows();
      leftGridRowData.filter((obj:any)=>obj.data.pickQty);        
      let  pickQtyLen = leftGridRowData.filter((obj:any)=>obj.data.pickQty === args.value).length;

        if (Number(args.value) > Number(leftGridRowData[e.row.rowIndex].data.quantity))          
        {
          alert("Invalid Pick Qty");
          this.shipmtLftTblVar.instance.cellValue(e.row.rowIndex, 3, '');                
            setTimeout(() => {
              this.shipmtLftTblVar.instance.focus(this.shipmtLftTblVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
            }, 300);         
        }
        else if (Number(args.value) < 0)  {
          alert("Invalid Pick Qty");
          this.shipmtLftTblVar.instance.cellValue(e.row.rowIndex, 3, '');                
            setTimeout(() => {
              this.shipmtLftTblVar.instance.focus(this.shipmtLftTblVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
            }, 300); 
        }
        else if (Number(args.value) < 0)  {
          alert("Invalid Pick Qty");
          this.shipmtLftTblVar.instance.cellValue(
            e.row.rowIndex, 3, '');                
            setTimeout(() => {
              this.shipmtLftTblVar.instance.focus(this.shipmtLftTblVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
            }, 300); 
        }
        else if (!Number(args.value))  {
          alert("Invalid Pick Qty");
          this.shipmtLftTblVar.instance.cellValue(
            e.row.rowIndex, 3, '');                
            setTimeout(() => {
              this.shipmtLftTblVar.instance.focus(this.shipmtLftTblVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
            }, 300); 
        }
        else if (args.value.length === 0) {
          alert("Enter Pick Qty");
          this.shipmtLftTblVar.instance.cellValue(
            e.row.rowIndex, 3, '');                
            setTimeout(() => {
              this.shipmtLftTblVar.instance.focus(this.shipmtLftTblVar.instance.getCellElement(e.row.rowIndex, "pickQty"));
            }, 300); 
        }
        else{                  
          this.shipmtLftTblVar.instance.cellValue(
            e.row.rowIndex,
            3,
            Number(args.value)
          );            
        }
      }.bind(this);
    }  
  }

  moveToRight(){
    let action = 1;
    let shipmentVisibleRows: ShipmentViewResult[] = [];
    let pickQtyVar=[];
    if (this.selectedRowView.invType === 'BASIC'){
        let chgLocRows = this.shipmtLftTblVar.instance.getVisibleRows();              
                
        for (let _i = 0; _i < chgLocRows.length; _i++) {                  
          if (chgLocRows[_i].data.pickQty !== undefined) {   
            shipmentVisibleRows.push(chgLocRows[_i].data);
          }            
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
          this.shipmtLftTblVar.instance.getCellElement(0, 3);
          return false;
        }
 
      if (shipmentVisibleRows.length > 0)
            this.shipViewLftTblDetails = shipmentVisibleRows;
          
        
      if (shipmentVisibleRows.length > 0){
        for (let _i = 0; _i < this.shipViewLftTblDetails.length; _i++) {      
          
                let transItem = new ShipmentPickShipItem();
                transItem.action = action;                              
                transItem.availQuantity = Number(this.shipViewLftTblDetails[_i].quantity);
                if (Number(this.shipViewLftTblDetails[_i].pickQty) !== undefined){
                  transItem.transQuantity = Number(this.shipViewLftTblDetails[_i].pickQty);                 
                }
                else{
                  alert("Please enter PickQty");
                  return;
                }
              
                transItem.soLinesId = this.selectedRowView.soLinesId;
                transItem.invBasicId = this.shipViewLftTblDetails[_i].invBasicId;
                transItem.invSerialId = this.shipViewLftTblDetails[_i].invSerialId;
                transItem.id = "";
                transItem.createdBy = 'admin';                     
                this.shipInvPickShipItem.push(transItem);               
        }  
      }   
    }
    else {
      console.log(this.leftSelectedItemKeys.length );
      if (this.leftSelectedItemKeys.length > 0){
        for (let _i = 0; _i < this.leftSelectedItemKeys.length; _i++) {      
                let transItem = new ShipmentPickShipItem();
                transItem.action = action;
                transItem.transQuantity = 1;                
                transItem.availQuantity = 1;
                transItem.soLinesId = this.selectedRowView.soLinesId;
                transItem.invBasicId = this.leftSelectedItemKeys[_i].invBasicId;
                transItem.invSerialId = this.leftSelectedItemKeys[_i].invSerialId;
                transItem.id = "";
                transItem.createdBy = 'admin';                    
                this.shipInvPickShipItem.push(transItem);   
        }  
      }
    }    
    this.updateTempInvPickShipTable();    
  }

  updateTempInvPickShipTable() {
    if (this.shipInvPickShipItem.length > 0)
    {
      this.shipmtService.UpdateInvPickShip(this.shipInvPickShipItem).subscribe(r => {
        console.log(r);
       // this.refreshDataGrid();

        setTimeout(() => {         
          this.getDetails();   
         }, 100);
        
      }, err => { notify({ message: "Error occured during move to right table", shading: true, position: top }, "error", 1500) });
      this.shipInvPickShipItem = [];
    }
  }

  moveToLeft(){
    let action = 2;   
     
    if (this.rightSelectedItemKeys.length > 0){
      for (let _i = 0; _i < this.rightSelectedItemKeys.length; _i++) {      
              let transItem = new ShipmentPickShipItem();
              transItem.action = action;
              transItem.transQuantity = 1;                
              transItem.availQuantity = 1;
              transItem.soLinesId = this.selectedRowView.soLinesId;
              transItem.invBasicId = this.rightSelectedItemKeys[_i].invBasicId;
              transItem.invSerialId = this.rightSelectedItemKeys[_i].invSerialId;
              transItem.id = "";
              transItem.createdBy = 'admin';                    
              this.shipInvPickShipItem.push(transItem);   
      }  
    }
    this.updateTempInvPickShipTable();
  }

  ship()
  {    
    console.log("mdatout " + this.shipmtSearchSelected.mdatOut + " projname " + this.shipmtSearchSelected.projectName); 

    if (this.shipmtSearchSelected.mdatOut === undefined || this.shipmtSearchSelected.mdatOut === "") 
    {
      notify({ message: "Mdat Out are required", shading: true, position: top }, "error", 1500);
    } 
    else 
    {
     
      console.log("shiplen " + this.shipViewRightTblDetails.length);

      if (this.shipViewRightTblDetails.length !== 0) {
        this.shipmtService.UpdateShipment("admin", this.shipmtSearchSelected.mdatOut).subscribe(r => { 
          console.log(r);
         // this.getDetails(); 
      });       
      
      alert("Successfully Shipped");
      this.shipmtService.DeleteInvPickShip("admin").subscribe(r => { console.log(r) });
      setTimeout(() => {
        this.getDetails();   
       }, 500);
      }
    }
  } 
}