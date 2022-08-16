import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { DxDataGridComponent } from 'devextreme-angular';
import { pmDetails } from 'src/app/pages/models/pmdetails';
import { pmNotes } from 'src/app/pages/models/pmNotes';
import { pmItemSearchResult } from 'src/app/pages/models/pmsearch';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent implements OnInit {
  @ViewChild(DxDataGridComponent, { static: false })
  dataGrid!: DxDataGridComponent;

  @Input() readOnly: boolean = true;
  @Input() poNotes: pmNotes[] = [];
  @Input() lookupItemNumbers: pmItemSearchResult[] = [];
  @Input() selectedRowIndex = -1;
  @Input() pmDetails: pmDetails = new pmDetails();

  notesDefaultRow: number = 2;

  constructor() {
    this.onReorder = this.onReorder.bind(this);
    this.onSaving = this.onSaving.bind(this);
    this.rowInserted = this.rowInserted.bind(this);
    this.rowUpdated = this.rowUpdated.bind(this);
    this.rowRemoved = this.rowRemoved.bind(this);
    this.onKeyDown = this.onKeyDown.bind(this);
    this.selectedChanged = this.selectedChanged.bind(this);
  }

  ngOnInit() {
  }

  logEvent(eventName: any) {
    console.log(eventName);
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
      //this.popupF2Visible = true;
    }
  }

  addRow() {
    this.dataGrid.instance.addRow();
    this.dataGrid.instance.deselectAll();
  }

  rowUpdated(e: any) {
    console.log(e);
    let key = e.key;
    let newData = e.newData;
    let oldData = e.oldData;
    var item = this.lookupItemNumbers.find(
      (i) => i.itemNumber == newData.itemNumber
    );
    if (!!item) {
      // let bitem = this.bomDetails.find((b) => b.itemNumber === key);
      // if (!!bitem) {
      //   bitem.itemNumber = item.itemNumber;
      //   bitem.itemtype = item.itemType;
      //   bitem.description = item.description;
      //   bitem.itemsid_Parent = Guid.parse(this.pmDetails.id);
      //   bitem.itemsid_Child = item.id;
      //   bitem.rev = item.rev;
      //   bitem.uomref = item.uomref;
      //   bitem.cost = item.cost;
      //   bitem.lineNum = bitem.lineNum;
      //   if (!!newData.quantity) {
      //     bitem.quantity = newData.quantity;
      //   }
      // }
    }
  }

  rowRemoved(e: any) {
    console.log(e);
    let lineNum = e.data.lineNum;
    // if (this.bomDetails.length > 0) {
    //   for (let i = lineNum; i <= this.bomDetails.length; i++) {
    //     this.bomDetails[i - 1].lineNum = i;
    //   }
    // }
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
      // let bitem = this.bomDetails.find((b) => b.itemNumber === key);
      // if (!!bitem) {
      //   bitem.id = '00000000-0000-0000-0000-000000000000';
      //   bitem.itemNumber = item.itemNumber;
      //   bitem.itemtype = item.itemType;
      //   bitem.description = item.description;
      //   bitem.itemsid_Parent = Guid.parse(this.pmDetails.id);
      //   bitem.itemsid_Child = item.id;
      //   bitem.rev = item.rev;
      //   bitem.uomref = item.uomref;
      //   bitem.cost = item.cost;
      //   //bitem.lineNum = this.bomDetails.length;
      //   if (!!newData.quantity) {
      //     bitem.quantity = newData.quantity;
      //   }
      // }
    }
  }

  selectedChanged(e: any) {
    console.log(e);
    this.selectedRowIndex = e.component.getRowIndexByKey(e.selectedRowKeys[0]);
  }

  onReorder(e: any) {
    const visibleRows = e.component.getVisibleRows();
    //const toIndex = this.bomDetails.indexOf(visibleRows[e.toIndex].data);
    //const fromIndex = this.bomDetails.indexOf(e.itemData);

    // this.bomDetails.splice(fromIndex, 1);
    // this.bomDetails.splice(toIndex, 0, e.itemData);
    // if (fromIndex < toIndex) {
    //   for (let i = fromIndex; i < toIndex; i++) {
    //     this.bomDetails[i].lineNum = this.bomDetails[i].lineNum - 1;
    //   }
    // } else {
    //   for (let i = fromIndex; i > toIndex; i--) {
    //     this.bomDetails[i].lineNum = this.bomDetails[i].lineNum + 1;
    //   }
    // }
    // this.bomDetails[toIndex].lineNum = toIndex + 1;
  }

  onSaving(e: any) {
    console.log('onSaving');
  }

  AddNotesLines() {

    //const dataSource = this.dataGrid.instance.getDataSource();
    // console.log( dataSource.items().length);
    debugger
    for (let i = 0; i < this.notesDefaultRow; i++) {
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

}
