import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { DxDataGridComponent } from 'devextreme-angular';
import notify from 'devextreme/ui/notify';
import { Guid } from 'guid-typescript';
import { ComponentType } from 'src/app/pages/models/componentType';
import { LabelMap } from 'src/app/pages/models/Label';
import { pmBomDetails } from 'src/app/pages/models/pmBomDetails';
import { pmDetails } from 'src/app/pages/models/pmdetails';
import { pmItemSearchResult, pmSearch } from 'src/app/pages/models/pmsearch';
import { SearchService } from 'src/app/pages/services/search.service';
import { TextboxStyle } from '../../textbox-style/textbox-style';

@Component({
  selector: 'app-bom',
  templateUrl: './bom.component.html',
  styleUrls: ['./bom.component.scss']
})
export class BomComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false })
  dataGrid!: DxDataGridComponent;
  @ViewChild('f2partNumberVar', { static: false }) f2partNumberVar! : ElementRef;
  @ViewChild('f2partNumberDesc', { static: false }) f2partNumberDesc! : ElementRef;


  @Input() readOnly: boolean = true;
  @Input() bomDetails: pmBomDetails[] = [];
  @Input() lookupItemNumbers: pmItemSearchResult[] = [];
  @Input() selectedRowIndex = -1;
  @Input() pmDetails: pmDetails = new pmDetails();

  StylingMode: string = TextboxStyle.StylingMode;
  LabelMode: string = TextboxStyle.LabelMode;
  labelMap: typeof LabelMap;

  pmSearchResults: pmItemSearchResult[] = [];
  bomDefaultRow: number = 2;
  popupF2Visible: boolean = false;
  basicPopupVisible: boolean = false;
  selectedItemNumber: string = '';
  selectedItem: any;
  f2KeyRowIndex = 0;
  componentTypeF2: ComponentType = ComponentType.PartMasterF2;

  constructor(private searchService: SearchService,) {
    this.labelMap = LabelMap;
    this.onReorder = this.onReorder.bind(this);
    this.onSaving = this.onSaving.bind(this);
    this.rowInserted = this.rowInserted.bind(this);
    this.rowUpdated = this.rowUpdated.bind(this);
    this.rowRemoved = this.rowRemoved.bind(this);
    this.onKeyDown = this.onKeyDown.bind(this);
    this.selectedChanged = this.selectedChanged.bind(this);
  }

  ngOnInit() {
    this.popupF2Visible = false;
  }

  onKeyDown(e: any) {
    if (this.readOnly) {
      return;
    }
    console.log(e);
    if (e.event.ctrlKey && e.event.key === 'ArrowDown') {
      this.dataGrid.instance.saveEditData();
      this.addRow();
    } else if (e.event.ctrlKey && e.event.key === 'F2') {
      this.popupF2Visible = true;
    }
  }

  addRow() {
    this.dataGrid.instance.addRow();
    this.dataGrid.instance.deselectAll();
  }

  logEvent(eventName: any) {
    console.log("event name: " + eventName);
  }

  onSaving(e: any) {
    console.log('onSaving');
  }

  AddBomLines() {

    //const dataSource = this.dataGrid.instance.getDataSource();
    // console.log( dataSource.items().length);

    for (let i = 0; i < this.bomDefaultRow; i++) {
      console.log('for--------');
      this.dataGrid?.instance.addRow();

    }

    let rows = this.dataGrid.instance.getVisibleRows();
    let rowCount = rows.length;

    let rowIndex = rows.find(obj => obj.data.itemNumber === undefined)?.rowIndex;

    console.log(rows);
    console.log(rowIndex);

    for (let i = 0; i < rowCount; i++) {
      this.dataGrid.instance.cellValue(i, 1, i + 1);
    }

    setTimeout(() => {
      this.dataGrid.instance.focus(this.dataGrid.instance.getCellElement(Number(rowIndex), "itemNumber") as HTMLElement);
    }, 300);

  }

  rowInserted(e: any) {
    console.log(e);
    let key = e.key;
    let newData = e.data;
    var item = this.lookupItemNumbers.find(
      (i) =>
        i.itemNumber.toLocaleLowerCase() ==
        newData.itemNumber.toLocaleLowerCase()
    );
    if (!!item) {
      let bitem = this.bomDetails.find((b) => b.itemNumber === key);
      if (!!bitem) {
        bitem.id = '00000000-0000-0000-0000-000000000000';
        bitem.itemNumber = item.itemNumber;
        bitem.itemtype = item.itemType;
        bitem.description = item.description;
        bitem.itemsid_Parent = Guid.parse(this.pmDetails.id);
        bitem.itemsid_Child = item.id;
        bitem.rev = item.rev;
        bitem.uomref = item.uomref;
        bitem.cost = item.cost;
        bitem.lineNum = this.bomDetails.length;
        if (!!newData.quantity) {
          bitem.quantity = newData.quantity;
        }
      }
    }
  }

  onEditorPreparing(e: any) {


    if (e.dataField === 'itemNumber' && e.parentType === 'dataRow') {


      const defaultValueChangeHandler = e.editorOptions.onValueChanged;

      e.editorOptions.onKeyDown = function (this: any, args: any) {

        if (args.event.keyCode == 113) {
          console.log(this);
          console.log(args);
          this.basicPopupVisible = true;
          this.f2KeyRowIndex = e.row.rowIndex;
        }
      }.bind(this);

      e.editorOptions.onValueChanged = function (this: any, args: any) {


        let rows = this.dataGrid.instance.getVisibleRows();

        let itemLen = rows.filter((obj: any) => obj.data.itemNumber?.toLowerCase() === args.value.toLowerCase())?.length;

        if (itemLen > 0) {
          alert('Item Number ' + args.value + ' already added');
          this.dataGrid.instance.cellValue(
            e.row.rowIndex,
            3,
            ''
          );
          setTimeout(() => {
            this.dataGrid.instance.focus(this.dataGrid.instance.getCellElement(e.row.rowIndex, "itemNumber"));
          }, 300);

        }
        else {

          let cellInfo = new pmSearch();
          cellInfo.itemnumber = args.value;

          this.searchService
            .getItemNumberSearchResults(cellInfo)
            .subscribe((response: pmItemSearchResult[]) => {


              let obj = response?.find(
                (x: pmItemSearchResult) =>
                  x.itemNumber.toLowerCase() == cellInfo.itemnumber.toLowerCase()
              );

              if (!!obj) {
                this.dataGrid.instance.cellValue(e.row.rowIndex, 2, obj.itemType);
                this.dataGrid.instance.cellValue(
                  e.row.rowIndex,
                  3,
                  obj.itemNumber
                );
                this.dataGrid.instance.cellValue(
                  e.row.rowIndex,
                  4,
                  obj.description
                );

                this.dataGrid.instance.cellValue(e.row.rowIndex, 6, obj.uomref);
                this.dataGrid.instance.cellValue(e.row.rowIndex, 8, obj.cost);

                this.dataGrid.instance.cellValue(e.row.rowIndex, 10, "00000000-0000-0000-0000-000000000000");
                this.dataGrid.instance.cellValue(e.row.rowIndex, 11, obj.id);

              } else {

                alert('Invalid Itemnumber');

                setTimeout(() => {
                  this.dataGrid.instance.focus(this.dataGrid.instance.getCellElement(e.row.rowIndex, "itemNumber"));
                }, 300);

              }
            });

        }

      }.bind(this);

    }

    if (e.dataField === 'quantity' && e.parentType === 'dataRow') {
      const defaultValueChangeHandler = e.editorOptions.onValueChanged;

      e.editorOptions.onValueChanged = function (this: any, args: any) {

        let costElement = this.dataGrid.instance.getCellElement(e.row.rowIndex, "cost");
        this.dataGrid.instance.cellValue(
          e.row.rowIndex,
          5,
          Number(args.value)
        );
        this.dataGrid.instance.cellValue(e.row.rowIndex, 9, e.row.data.cost * Number(args.value));
        //this.dataGrid.instance.saveEditData();

      }.bind(this);
    }


    if (e.dataField === 'ref' && e.parentType === 'dataRow') {

      const defaultValueChangeHandler = e.editorOptions.onb;

      e.editorOptions.onFocusOut = function (this: any, args: any) {
        if (e.row.rowIndex === this.dataGrid.instance.getVisibleRows().length - 1) {
          this.AddBomLines();
        }

      }.bind(this);
    }



  }

  selectedChanged(e: any) {
    console.log(e);
    this.selectedRowIndex = e.component.getRowIndexByKey(e.selectedRowKeys[0]);
  }

  onInitialized(e: any) {

    console.log('onInitialized');
    //this.AddBomLines();
  }

  onToolbarPreparing(e: any) {
    debugger
    e.toolbarOptions.visible = false;
  }

  rowUpdated(e: any) {
    debugger
    console.log(e);
    let key = e.key;
    let newData = e.newData;
    let oldData = e.oldData;
    var item = this.lookupItemNumbers.find(
      (i) => i.itemNumber == newData.itemNumber
    );
    if (!!item) {
      let bitem = this.bomDetails.find((b) => b.itemNumber === key);
      if (!!bitem) {
        bitem.itemNumber = item.itemNumber;
        bitem.itemtype = item.itemType;
        bitem.description = item.description;
        bitem.itemsid_Parent = Guid.parse(this.pmDetails.id);
        bitem.itemsid_Child = item.id;
        bitem.rev = item.rev;
        bitem.uomref = item.uomref;
        bitem.cost = item.cost;
        bitem.lineNum = bitem.lineNum;
        if (!!newData.quantity) {
          bitem.quantity = newData.quantity;
        }
      }
    }
  }

  rowRemoved(e: any) {
    alert("row removing")
    console.log(e);
    let lineNum = e.data.lineNum;
    if (this.bomDetails.length > 0) {
      for (let i = lineNum; i <= this.bomDetails.length; i++) {
        this.bomDetails[i - 1].lineNum = i;
      }
    }
  }

  test(e: any) {
    console.log(e);
    console.log(e.row.data);
  }

  onReorder(e: any) {
    const visibleRows = e.component.getVisibleRows();
    const toIndex = this.bomDetails.indexOf(visibleRows[e.toIndex].data);
    const fromIndex = this.bomDetails.indexOf(e.itemData);

    this.bomDetails.splice(fromIndex, 1);
    this.bomDetails.splice(toIndex, 0, e.itemData);
    if (fromIndex < toIndex) {
      for (let i = fromIndex; i < toIndex; i++) {
        this.bomDetails[i].lineNum = this.bomDetails[i].lineNum - 1;
      }
    } else {
      for (let i = fromIndex; i > toIndex; i--) {
        this.bomDetails[i].lineNum = this.bomDetails[i].lineNum + 1;
      }
    }
    this.bomDetails[toIndex].lineNum = toIndex + 1;
  }

  onShown(e: any): void {
    //this.chart.render();
    console.log('OnPNSearchShow2');
  }

  findPartNumbers(pn: string, desc: string) {
    console.log(pn);
    console.log(desc);
    let obj: pmSearch = new pmSearch();
    obj.itemnumber = pn;
    obj.description = desc;
    this.searchService.getItemNumberSearchResults(obj).subscribe(r => {
      this.pmSearchResults = r;
    });
  }

  onSelectionChanged(e: any) {
    console.log(e);
    // this.f2partNumberVar.nativeElement.value = "";
    // this.f2partNumberDesc.nativeElement.value = "";        
    if (!!e.addedItems[0]) {
      this.selectedItem = e.addedItems[0];
      console.log(this.selectedItem);
      this.selectedItemNumber = this.selectedItem.itemNumber;
      // this.pmDataTransService.selectedItemChanged(this.selectedItem, this.componentType);

      this.dataGrid.instance.cellValue(
        this.f2KeyRowIndex,
        3,
        this.selectedItemNumber
      );
      this.basicPopupVisible = false;


      let cellInfo = new pmSearch();
      cellInfo.itemnumber = this.selectedItemNumber;

      this.searchService
        .getItemNumberSearchResults(cellInfo)
        .subscribe((response: pmItemSearchResult[]) => {


          let obj = response?.find(
            (x: pmItemSearchResult) =>
              x.itemNumber.toLowerCase() == cellInfo.itemnumber.toLowerCase()
          );

          console.log(obj);

          if (!!obj) {
            this.dataGrid.instance.cellValue(this.f2KeyRowIndex, 2, obj.itemType);
            this.dataGrid.instance.cellValue(
              this.f2KeyRowIndex,
              3,
              obj.itemNumber
            );
            this.dataGrid.instance.cellValue(
              this.f2KeyRowIndex,
              4,
              obj.description
            );

            this.dataGrid.instance.cellValue(this.f2KeyRowIndex, 6, obj.uomref);
            this.dataGrid.instance.cellValue(this.f2KeyRowIndex, 8, obj.cost);

            this.dataGrid.instance.cellValue(this.f2KeyRowIndex, 10, "00000000-0000-0000-0000-000000000000");
            this.dataGrid.instance.cellValue(this.f2KeyRowIndex, 11, obj.id);

          }
        })
      setTimeout(() => {
        this.dataGrid.instance.focus(this.dataGrid.instance.getCellElement(Number(this.f2KeyRowIndex), "quantity") as HTMLElement);
      }, 300);
    }
  }

  dblclick(e: any){
    // console.log('OnPNSearchShow');
     //this.f2partNumberVar.nativeElement.focus();
    // console.log('OnPNSearchShow2');
   }
}
