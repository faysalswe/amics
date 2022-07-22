import { Component, ViewChild } from "@angular/core";
import notify from "devextreme/ui/notify";
import { Guid } from "guid-typescript";
import { pmBomDetails } from "src/app/pages/models/pmBomDetails";
import { BomAction, pmBomGridDetails } from "src/app/pages/models/pmBomGridDetails";
import { CRUD, PmChildType, PopUpAction } from "src/app/pages/models/pmChildType";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { pmPoDetails } from "src/app/pages/models/pmPoDetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmItemSearchResult, pmSearch } from "src/app/pages/models/pmsearch";
import { pmWHLocation } from "src/app/pages/models/pmWHLocation";
import { ItemClass, ItemCode, ItemType, Uom } from "src/app/pages/models/searchModels";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { AuthService } from "src/app/shared/services";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";
import { DxDataGridComponent } from "devextreme-angular";
import { pmSerial } from "src/app/pages/models/pmSerial";
import { Workbook } from "exceljs";
import { saveAs } from "file-saver";
import { exportDataGrid } from "devextreme/excel_exporter";
import { pmNotes } from "src/app/pages/models/pmNotes";
import { ComponentType } from "src/app/pages/models/componentType";
import { ThisReceiver } from "@angular/compiler";
@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {
    @ViewChild(DxDataGridComponent, { static: false }) dataGrid!: DxDataGridComponent
    secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
    warehouses: Warehouse[] = [];
    warehouseNames: string[] = [];
    pmWHLocations: pmWHLocation[] = [];
    pmSerials: pmSerial[] = [];
    pmNotes: pmNotes[] = [];
    groupedLocations: any;
    groupedWarehouses: any;
    locations: WarehouseLocation[] = [];
    validLocationNames: string[] = [];
    bomDetails: pmBomDetails[] = [];
    originalBomDetails: pmBomDetails[] = [];
    poDetails: pmPoDetails[] = [];
    poNotes: pmNotes[] = [];
    reasonsDelete: string[] = [];
    selectedChild: PmChildType = PmChildType.BOM;
    childType: typeof PmChildType;
    readOnly: boolean = true;
    itemClassList: ItemClass[] = [];
    itemCodeList: ItemCode[] = [];
    itemTypeList: ItemType[] = [];
    uomList: Uom[] = [];
    yesButtonOptions: any;
    noButtonOptions: any;
    yesDeleteButtonOptions: any;
    noDeleteButtonOptions: any;
    viewLocationPrintButtonOptions: any;
    popupCopyBomVisible = false;
    popupDeleteVisible = false;
    popupDeleteMessages = false;
    copyToNewClicked: boolean = false;
    toastVisible = false;
    toastType = 'info';
    toastMessage = '';
    popupVLVisible = false;
    popupF2Visible = false;
    popupVSVisible = false;
    lookupItemNumbers: pmItemSearchResult[] = [];
    selectedRowIndex = -1;
    editRowKey!: number;
    componentTypeF2: ComponentType = ComponentType.PartMasterF2;
    warehouse: string = '';
    location: string = '';
    constructor(private searchService: SearchService, private pmdataTransfer: PartMasterDataTransService, private pmService: PartMasterService, private authService: AuthService) {
        this.childType = PmChildType;
        this.searchService.getWarehouseInfo('').subscribe(w => {
            this.warehouses = w;
            this.warehouseNames = w.map(w => w.warehouse);
            this.groupedWarehouses = this.groupByKey(w, 'warehouse');           
        })

        this.searchService.getLocationInfo('', '').subscribe(l => {
            this.locations = l;
            this.groupedLocations = this.groupByKey(l, 'warehouseId');
            console.log(this.groupedLocations);
            //   console.log(this.groupedLocations['f062f282-ad8e-4743-b01f-2fb9c7ba9f7d']);
        })

        const that = this;
        this.yesButtonOptions = {
            text: 'Yes',
            onClick(e: any) {
                that.popupCopyBomVisible = false;
                console.log('Yes to Copy Bom');
                that.pmDetails.id = Guid.EMPTY;
                that.pmDetails.itemNumber = '';
                that.copyToNewClicked = true;
            },
        };
        this.noButtonOptions = {
            text: 'No',
            onClick(e: any) {
                that.popupCopyBomVisible = false;
                that.copyToNewClicked = false;
            }
        };

        this.yesDeleteButtonOptions = {
            text: 'Yes',
            onClick(e: any) {
                that.popupDeleteVisible = false;
                console.log('Yes to Delete partnumber');
                that.onDelete();
            },
        };
        this.noDeleteButtonOptions = {
            text: 'No',
            onClick(e: any) {
                that.popupDeleteVisible = false;
                that.copyToNewClicked = false;
            }
        };
        this.viewLocationPrintButtonOptions = {
            text: 'Print',
            onClick(e: any) {
                that.popupVLVisible = false;
            }
        };
        this.onReorder = this.onReorder.bind(this);
        this.validateItem = this.validateItem.bind(this);
        this.onSaving = this.onSaving.bind(this);
        this.rowInserted = this.rowInserted.bind(this);
        this.rowUpdated = this.rowUpdated.bind(this);
        this.rowRemoved = this.rowRemoved.bind(this);
        this.onKeyDown = this.onKeyDown.bind(this);
        this.selectedChanged = this.selectedChanged.bind(this);
        this.setCellValue = this.setCellValue.bind(this);
    }
    logEvent(eventName: any) {
        console.log(eventName);
    }
    onExporting(e: any) {
        const workbook = new Workbook();
        const worksheet = workbook.addWorksheet('pmWHLocations');

        exportDataGrid({
            component: e.component,
            worksheet,
            autoFilterEnabled: true,
        }).then(() => {
            workbook.xlsx.writeBuffer().then((buffer) => {
                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
            });
        });
        e.cancel = true;
    }
    ngOnInit(): void {
        this.searchService.getItemClass('', '').subscribe(l => {
            this.itemClassList = l;
        })

        this.searchService.getItemCode('', '').subscribe(l => {
            this.itemCodeList = l;
        })

        this.searchService.getItemType('', '').subscribe(l => {
            this.itemTypeList = l;
        })
        this.searchService.getUom('', '').subscribe(l => {
            this.uomList = l;
        })
        this.pmdataTransfer.itemSelectedSubjectForBomGrid$.subscribe(e => {
            if (this.readOnly) { return; }
            console.log(e);
            this.popupF2Visible = false;
            this.dataGrid.instance.saveEditData();
            this.addRow();
            this.dataGrid.instance.cellValue(this.bomDetails.length, 3, e.itemNumber);
            this.dataGrid.instance.cellValue(this.bomDetails.length, 5, 1);
            this.dataGrid.instance.saveEditData();
            this.editRow(e.itemNumber);

        });
        this.pmdataTransfer.selectedItemForPMDetails$.subscribe(item => {
            console.log(item);
            this.pmDetails = item;
            this.copyToNewClicked = false;
            this.updateWarehouseSelection(item.warehouse, true);
            this.pmdataTransfer.isSerialSelected$.next(item.invType == 'SERIAL');
        });
        this.pmdataTransfer.selectedItemBomForPMDetails$.subscribe(boms => {
            this.bomDetails = boms;
            this.originalBomDetails = [...boms];
        })

        this.pmdataTransfer.selectedItemPoForPMDetails$.subscribe(poLines => {
            this.poDetails = poLines;
        })

        this.pmdataTransfer.selectedItemNotesForPMDetails$.subscribe(poNotes => {
            this.poNotes = poNotes;
        })

        this.pmdataTransfer.itemSelectedChild$.subscribe(child => { this.selectedChild = child; });
        this.pmdataTransfer.selectedCRUD$.subscribe(crud => {
            if (crud === CRUD.Add) {
                this.pmDetails = new pmDetails();
                this.bomDetails = [];
                this.poDetails = [];
                this.readOnly = false;
            }
            else if (crud === CRUD.Edit) {
                this.readOnly = false;
            }
            else if (crud === CRUD.Save) {
                this.onSave();
                this.readOnly = true;
            } else if (crud === CRUD.Delete) {
                // this.onDelete();
                this.popupDeleteVisible = true;
                this.readOnly = true;
            } else {
                this.readOnly = true;
            }
        });
        this.pmdataTransfer.selectedPopUpAction$.subscribe(popUp => {
            console.log(popUp);
            if (popUp === PopUpAction.UF) {

            }
            else if (popUp === PopUpAction.VL) {
                this.popupVLVisible = true;
                this.getLocations();
            }
            else if (popUp === PopUpAction.VS) {
                this.popupVSVisible = true;
                this.getSerial();
            } else if (popUp === PopUpAction.Print) {

            }
        });
        this.pmdataTransfer.copyToNewSelected$.subscribe(e => this.popupCopyBomVisible = true);
        this.getListItemNumbers();
    }

    getLocations(wh: string = '') {
        this.pmService.getViewWHLocation(this.pmDetails.id, this.secUserId, wh).subscribe(
            x => this.pmWHLocations = x
        )
    }
    getSerial() {
        this.pmService.getViewSerial(this.pmDetails.id, this.secUserId).subscribe(
            x => this.pmSerials = x
        )
    }
    getNotes() {
        this.pmService.getViewNotes(this.pmDetails.id).subscribe(
            x => this.pmNotes = x
        )
    }
    //  groupByKey = (list:any, key:any) => list.reduce((hash:any, obj:any) => ({...hash, [obj[key]]:( hash[obj[key]] || [] ).concat(obj)}), {})
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
            //this.pmDetails.location = '';
            this.location = '';
            return;        
        }
       
        let wid = this.groupedWarehouses[this.warehouse];
        if (!!wid) {           
            let locations: WarehouseLocation[] = this.groupedLocations[wid[0].id];
            this.validLocationNames = locations.map(l => l.location);
        } else { this.validLocationNames = []; }
    }

    WarehouseLocationSelection(e: any) {
        console.log(e);
        if (e.value === null) {
            this.getLocations('');
        }
        else {
            this.getLocations(e.value);
        }
    }

    ItemNumberSelection(e: any) {
        console.log(e);
        //   this.getListItemNumbers();
    }

    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default"
    }
    pmDetails: pmDetails = new pmDetails();
    pmpoviewArray: PMPOView[] = [];
    invTypes: string[] = ["BASIC", "SERIAL"];
    warehouseLbl: string = "Warehouse";
    locationLbl: string = "Location";
    handleSubmit = function (e: any) {
        setTimeout(() => {
            alert("Submitted");
        }, 1000);

        e.preventDefault();
    }
    isFormValid() {
        let anyErrors = false;
        let msg = "";

        if (this.pmDetails.itemNumber === '') {
            console.log("Invalid PartNumber");
            anyErrors = true;
            msg = msg + "Invalid PartNumber |";
        }
        if (this.pmDetails.itemType === '') {
            console.log("Invalid MFR");
            anyErrors = true;
            msg = msg + "Invalid MFR |";
        }
        var uomid = this.uomList.find(u => u.uom === this.pmDetails.uomref)?.id ?? Guid.EMPTY;

        if (uomid === Guid.EMPTY) {
            console.log("Invalid UOM");
            anyErrors = true;
            msg = msg + "Invalid UOM |";
        }
        if (this.pmDetails.warehouse === '') {
            console.log("Invalid Warehouse");
            anyErrors = true;
            msg = msg + "Invalid Warehouse |";
        }
        if (this.pmDetails.location === '') {
            console.log("Invalid Location");
            anyErrors = true;
            msg = msg + "Invalid Location ";
        }

        if (anyErrors) {
            notify({ message: msg, shading: true, position: top }, "error", 1000);
            // this.toastType = "error";
            // this.toastVisible = true;
            // this.toastMessage = msg;
        }

        return !anyErrors;
    }
    buttonOptions: any = {
        text: 'submit',
        type: 'success',
        useSubmitBehavior: true,
    };
    onSave() {
        document.getElementById("pmDetailsSubmit")?.click();
        if (!this.isFormValid()) {
            return;
        }

        var uomid = this.uomList.find(u => u.uom === this.pmDetails.uomref)?.id ?? Guid.EMPTY;
        if (uomid === Guid.EMPTY) {
            console.log("Invalid UOM");
            notify({ message: "Invalid UOM", shading: true, position: top }, "error", 500);
        }

        this.pmService.addorUpdatePMDetails(this.pmDetails, uomid).subscribe(x => {
            if (this.copyToNewClicked) {
                var boms = this.copyToNewBomGridDetails(x.message);
                this.pmService.AddUpdateDeleteBomDetails(boms).subscribe(b => { notify({ message: b.message, shading: true, position: top }, "success", 500) }, err => {
                    notify({ message: "error while saving bom", shading: true },
                        "error", 1000)
                });
            } else {

                var boms = this.convertToBomGridDetails(x.message);
                this.pmService.AddUpdateDeleteBomDetails(boms).subscribe(b => { notify({ message: b.message, shading: true, position: top }, "success", 500) }, err => {
                    notify({ message: "error while saving bom", shading: true },
                        "error", 1000)
                });

            }
        },
            (err) => { notify({ message: "error occurred while saving", shading: true }, "error", 500) });
    }

    convertToBomGridDetails(parentId: string) {
        let bomGridDetails: pmBomGridDetails[] = [];

        if (this.bomDetails.length == 0 && this.originalBomDetails.length == 0) {
            return bomGridDetails;
        }

        // find inserted 
        let newBoms = this.bomDetails.filter(b => b.id === "00000000-0000-0000-0000-000000000000");
        if (!!newBoms && newBoms.length > 0) {
            for (let _i = 0; _i < newBoms.length; _i++) {
                let newBom = new pmBomGridDetails();
                newBom.actionFlag = BomAction.Add;
                newBom.parent_ItemsId = parentId;
                newBom.child_ItemsId = newBoms[_i].itemsid_Child.toString();
                newBom.lineNum = newBoms[_i].lineNum;
                newBom.quantity = newBoms[_i].quantity.toFixed(2);
                newBom.ref = newBoms[_i].ref;
                newBom.findNo = newBoms[_i].findNo;
                newBom.comments = newBoms[_i].comments;
                newBom.createdby = this.authService.currentUser.toString();
                bomGridDetails.push(newBom);
            }
        }

        //find Updated and deleted
        for (var _i = 0, boms = this.originalBomDetails; _i < boms.length; _i++) {
            var bom = new pmBomGridDetails();
            var originalBom = this.bomDetails.find(b => b.id == this.originalBomDetails[_i].id)
            if (!originalBom) {
                bom.actionFlag = BomAction.Delete;
            }
            else {
                bom.actionFlag = BomAction.Update
            }
            bom.id = boms[_i].id.toString();
            bom.parent_ItemsId = parentId;
            bom.child_ItemsId = boms[_i].itemsid_Child.toString();
            bom.lineNum = _i + 1;
            bom.quantity = boms[_i].quantity.toFixed(2);
            bom.ref = boms[_i].ref;
            bom.findNo = boms[_i].findNo;
            bom.comments = boms[_i].comments;
            bom.createdby = this.authService.currentUser.toString();
            bomGridDetails.push(bom);
        }

        return bomGridDetails;
    }

    copyToNewBomGridDetails(parentId: string) {
        let bomGridDetails: pmBomGridDetails[] = [];

        for (var _i = 0, boms = this.bomDetails; _i < boms.length; _i++) {
            var bom = new pmBomGridDetails();
            bom.actionFlag = BomAction.Add;
            bom.parent_ItemsId = parentId;
            bom.child_ItemsId = boms[_i].itemsid_Child.toString();
            bom.lineNum = _i + 1;
            bom.quantity = boms[_i].quantity.toFixed(2);
            bom.ref = boms[_i].ref;
            bom.findNo = boms[_i].findNo;
            bom.comments = boms[_i].comments;
            bom.createdby = this.authService.currentUser.toString();
            bomGridDetails.push(bom);
        }

        return bomGridDetails;
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
    onDelete() {
        this.pmService.deletePMDetails(this.pmDetails.itemNumber, this.pmDetails.rev).subscribe(x => {
            if (x.length === 0) {
                notify({ message: "deleted successfully", shading: true, position: top }, "success", 1000);
                this.pmdataTransfer.selectedCRUDActionChanged(CRUD.DoneDelete, ComponentType.PartMaster);
            } else {
                this.reasonsDelete = x;
                this.popupDeleteMessages = true;
            }

        });
    }
    getItemDetilsForBom(e: any) {
        console.log(e);
        let item;
    }
    getListItemNumbers() {
        var search = new pmSearch();
        this.searchService.getItemNumberSearchResults(search).subscribe(x => this.lookupItemNumbers = x);
    }

    validateItem(e: any) {
        console.log(e);
        var item = this.lookupItemNumbers.find(u => u.itemNumber.toLocaleLowerCase() === e.value.toLocaleLowerCase());
        if (!!item) {
            e.data.description = item.description;
            e.data.itemType = item.itemType;
            e.data.uomref = item.uomref;
            e.data.cost = item.cost;
            e.data.dwgNo = item.dwgNo;
            return true;
        }
        else {
            return false;
        }
    }
    rowUpdating(e: any) {
        console.log("rowUpdating");
        console.log(e);
        return true;
    }
    onEditingStart(e: any) {
        console.log(e);
        this.editRowKey = e.key;
        // ...
    }
    rowUpdated(e: any) {
        console.log(e);
        let key = e.key;
        let newData = e.newData;
        let oldData = e.oldData;
        var item = this.lookupItemNumbers.find(i => i.itemNumber == newData.itemNumber);
        if (!!item) {
            let bitem = this.bomDetails.find(b => b.itemNumber === key);
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
        console.log(e);
        let lineNum = e.data.lineNum;
        if (this.bomDetails.length > 0) {
            for (let i = lineNum; i <= this.bomDetails.length; i++) {
                this.bomDetails[i - 1].lineNum = i;
            }
        }
    }
    rowInserted(e: any) {
        console.log(e);
        let key = e.key;
        let newData = e.data;
        var item = this.lookupItemNumbers.find(i => i.itemNumber.toLocaleLowerCase() == newData.itemNumber.toLocaleLowerCase());
        if (!!item) {
            let bitem = this.bomDetails.find(b => b.itemNumber === key);
            if (!!bitem) {
                bitem.id = "00000000-0000-0000-0000-000000000000";
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
    onSaving(e: any) {
        console.log("onSaving");
    }

    getCellValue() {
        const editRowIndex = this.dataGrid.instance.getRowIndexByKey(this.editRowKey);
        if (editRowIndex >= 0) {
            return this.dataGrid.instance.cellValue(editRowIndex, "itemNumber");
        }
        return null;
    }
    onKeyDown(e: any) {
        if (this.readOnly) { return; }
        console.log(e);
        if (e.event.ctrlKey && e.event.key === "ArrowDown") {
            this.dataGrid.instance.saveEditData();
            this.addRow();
        } else if (e.event.ctrlKey && e.event.key === "F2") {
            this.popupF2Visible = true;
        }
    }

    addRow() {
        this.dataGrid.instance.addRow();
        this.dataGrid.instance.deselectAll();
    }

    selectedChanged(e: any) {
        console.log(e);
        this.selectedRowIndex = e.component.getRowIndexByKey(e.selectedRowKeys[0]);
    }
    editRow(key: any) {
        //let selectedRowIndex = this.dataGrid.instance.getRowIndexByKey(key);
        let selectedRowIndex = this.bomDetails.findIndex(b => b.itemNumber === key);
        this.dataGrid.instance.editRow(selectedRowIndex);
        this.dataGrid.instance.deselectAll();
    }
    setCellValue(newData: any, value: any, currentRowData: any) {
        newData.extCost = (currentRowData.cost * currentRowData.quantity);
        let selectedRowIndex = this.bomDetails.findIndex(b => b.itemNumber === currentRowData.itemNumber);
        this.dataGrid.instance.cellValue(selectedRowIndex, 5, currentRowData.quantity);
        this.dataGrid.instance.cellValue(selectedRowIndex, 6, newData.extCost);
    }
}

