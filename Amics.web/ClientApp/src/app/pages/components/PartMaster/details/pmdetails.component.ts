import { ThisReceiver } from "@angular/compiler";
import { Component, OnInit } from "@angular/core";
import notify from "devextreme/ui/notify";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { pmBomDetails } from "src/app/pages/models/pmBomDetails";
import { BomAction, pmBomGridDetails } from "src/app/pages/models/pmBomGridDetails";
import { CRUD, PmChildType } from "src/app/pages/models/pmChildType";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { pmPoDetails } from "src/app/pages/models/pmPoDetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { ItemClass, ItemCode, ItemType, Uom } from "src/app/pages/models/searchModels";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
import { AuthService } from "src/app/shared/services";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";

@Component({
    selector: "app-pmdetails",
    templateUrl: "./pmdetails.component.html",
    styleUrls: ['./pmdetails.component.scss']
})
export class PMDetailsComponent {
    warehouses: Warehouse[] = [];
    warehouseNames: string[] = [];
    groupedLocations: any;
    groupedWarehouses: any;
    locations: WarehouseLocation[] = [];
    validLocationNames: string[] = [];
    bomDetails: pmBomDetails[] = [];
    poDetails: pmPoDetails[] = [];
    selectedChild: PmChildType = PmChildType.BOM;
    childType: typeof PmChildType;
    readOnly: boolean = true;
    itemClassList: ItemClass[] = [];
    itemCodeList: ItemCode[] = [];
    itemTypeList: ItemType[] = [];
    uomList: Uom[] = [];
    yesButtonOptions: any;
    noButtonOptions: any;
    popupVisible = false;
    pmBomDataSource: any;
    copyToNewClicked: boolean = false;
    toastVisible = false;
    toastType = 'info';
    toastMessage = '';


    constructor(private searchService: SearchService, private pmdataTransfer: PartMasterDataTransService, private pmService: PartMasterService, private authService: AuthService) {
        this.childType = PmChildType;
        const that = this;
        this.yesButtonOptions = {
            text: 'Yes',
            onClick(e: any) {
                that.popupVisible = false;
                console.log('Yes to Copy Bom');
                that.pmDetails.id = Guid.EMPTY;
                that.pmDetails.itemNumber = '';
                that.copyToNewClicked = true;
            },
        };
        this.noButtonOptions = {
            text: 'No',
            onClick(e: any) {
                that.popupVisible = false;
                that.copyToNewClicked = false;
            }

        };
    }
    logEvent(eventName: any) {
        console.log(eventName);
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

        this.searchService.getWarehouseInfo('').subscribe(w => {
            this.warehouses = w;
            this.warehouseNames = w.map(w => w.warehouse);
            this.groupedWarehouses = this.groupByKey(w, 'warehouse');
        })

        this.searchService.getLocationInfo('', '').subscribe(l => {
            this.locations = l;
            this.groupedLocations = this.groupByKey(l, 'warehouseId');
            console.log(this.groupedLocations);
            console.log(this.groupedLocations['f062f282-ad8e-4743-b01f-2fb9c7ba9f7d']);
        })

        this.pmdataTransfer.selectedItemForPMDetails$.subscribe(item => {
            console.log(item);
            this.pmDetails = item;
            this.copyToNewClicked = false;
            this.updateWarehouseSelection(item.warehouse, true);
        });
        this.pmdataTransfer.selectedItemBomForPMDetails$.subscribe(boms => {
            this.bomDetails = boms;
        })

        this.pmdataTransfer.selectedItemPoForPMDetails$.subscribe(poLines => {
            this.poDetails = poLines;
        })

        this.pmdataTransfer.itemSelectedChild$.subscribe(child => { this.selectedChild = child; });
        this.pmdataTransfer.itemSelectedCRUD$.subscribe(crud => {
            if (crud === CRUD.Add) {
                this.pmDetails = new pmDetails();
                this.readOnly = false;
            }
            else if (crud === CRUD.Edit) {
                this.readOnly = false;
            }
            else if (crud === CRUD.Save) {
                this.onSave();
                this.readOnly = true;
            } else if (crud === CRUD.Delete) {
                this.onDelete();
                this.readOnly = true;
            } else {
                this.readOnly = true;
            }
        });
        this.pmdataTransfer.copyToNewSelected$.subscribe(e => this.popupVisible = true);
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
        if (!this.pmDetails.warehouse || !location) {
            this.validLocationNames = [];
            this.pmDetails.location = '';
            return;
        }

        let wid = this.groupedWarehouses[this.pmDetails.warehouse];
        if (!!wid) {
            let locations: WarehouseLocation[] = this.groupedLocations[wid[0].id];
            this.validLocationNames = locations.map(l => l.location);
        } else { this.validLocationNames = []; }

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
            notify({ message: msg ,shading:true, position:top}, "error", 1000);
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

        this.pmService.AddorUpdatePMDetails(this.pmDetails, uomid).subscribe(x => {
            if (this.copyToNewClicked) {
                this.pmService.getPartMaster(this.pmDetails.itemNumber, this.pmDetails.rev).subscribe((x) => {

                    var boms = this.convertToNewBomGridDetails(x.id);
                    this.pmService.AddUpdateDeleteBomDetails(boms).subscribe(b => { notify({ message: b, shading: true }, "success", 500) });

                });
            } else {
                notify({ message: x, shading: true }, "success", 500)
            }
        });
    }


    convertToNewBomGridDetails(parentId: string) {
        let bomGridDetails: pmBomGridDetails[] = [];

        for (var _i = 0, boms = this.bomDetails; _i < boms.length; _i++) {
            var bom = new pmBomGridDetails();
            bom.actionFlag = BomAction.Add;
            bom.parent_ItemsId = parentId;
            bom.lineNum = _i;
            bom.quantity = boms[_i].quantity.toFixed(2);
            bom.ref = boms[_i].ref;
            bom.findNo = boms[_i].findNo;
            bom.comments = boms[_i].comments;
            bom.createdby = this.authService.currentUser.toString();
            bomGridDetails.push(bom);
        }

        return bomGridDetails;
    }

    onDelete() {
        this.pmService.DeletePM(this.pmDetails.itemNumber, this.pmDetails.rev).subscribe(x => console.log(x + " deleted"));
    }

}