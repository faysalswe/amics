import { Component, OnInit } from "@angular/core";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { pmBomDetails } from "src/app/pages/models/pmBomDetails";
import { CRUD, PmChildType } from "src/app/pages/models/pmChildType";
import { pmDetails } from "src/app/pages/models/pmdetails";
import { pmPoDetails } from "src/app/pages/models/pmPoDetails";
import { PMPOView } from "src/app/pages/models/pmpoview";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { ItemClass, ItemCode, ItemType, Uom } from "src/app/pages/models/searchModels";
import { Warehouse, WarehouseLocation } from "src/app/pages/models/warehouse";
import { SearchService } from "src/app/pages/services/search.service";
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
    constructor(private searchService: SearchService, private pmdataTransfer: PartMasterDataTransService, private pmService: PartMasterService) {
        this.childType = PmChildType;
        const that = this;
        this.yesButtonOptions = {
            text: 'Yes',
            onClick(e:any) {
                that.popupVisible = false;
            },
        };
        this.noButtonOptions = {
            text: 'No',
            onClick(e:any) {
                that.popupVisible = false;
            }

        };
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

    onSave() {
        var uomid = this.uomList.find(u => u.uom === this.pmDetails.uomref)?.id ?? Guid.createEmpty();

        this.pmService.AddorUpdatePMDetails(this.pmDetails, uomid).subscribe(x => console.log(x +
            "saved"));
    }

    onDelete() {
        this.pmService.DeletePM(this.pmDetails.itemNumber, this.pmDetails.rev).subscribe(x => console.log(x + " deleted"));
    }

}