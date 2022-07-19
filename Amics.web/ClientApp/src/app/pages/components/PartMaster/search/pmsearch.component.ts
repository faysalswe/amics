import { ThisReceiver } from "@angular/compiler";
import { Component, Input, OnInit, AfterViewInit, ViewChild } from "@angular/core";
import { DxSelectBoxComponent } from "devextreme-angular";
import { ComponentType } from "src/app/pages/models/componentType";
import { LabelMap } from "src/app/pages/models/Label";
import { CRUD } from "src/app/pages/models/pmChildType";
import { pmSearch, pmItemSearchResult } from "src/app/pages/models/pmsearch";
import { ItemClass, ItemCode, ItemType } from "src/app/pages/models/searchModels";
import { SearchService } from "src/app/pages/services/search.service";
import { PartMasterService } from "../../../services/partmaster.service";
import { PartMasterDataTransService } from "../../../services/pmdatatransfer.service";
import { TextboxStyle } from "../../textbox-style/textbox-style";


@Component({
    selector: "app-pmsearch",
    templateUrl: "./pmsearch.component.html",
    styleUrls: ['./pmsearch.component.scss']
})
export class PMSearchComponent implements OnInit, AfterViewInit {
    @Input() componentType: ComponentType = ComponentType.PartMaster;
    @ViewChild('mfrVar', { static: false }) mfrVar!: DxSelectBoxComponent;
    @ViewChild('itemClassVar', { static: false }) itemClassVar!: DxSelectBoxComponent;
    @ViewChild('itemCodeVar', { static: false }) itemCodeVar!: DxSelectBoxComponent;
    pmsearchInfo: pmSearch = new pmSearch();
    pmSearchResults: pmItemSearchResult[] = [];
    submitButtonOptions = {
        text: "Search",
        useSubmitBehavior: true,
        width: "100%",
        type: "default",
    };
    StylingMode: string = TextboxStyle.StylingMode;
    LabelMode: string = TextboxStyle.LabelMode;
    itemClassList: ItemClass[] = [];
    itemCodeList: ItemCode[] = [];
    itemTypeList: ItemType[] = [];
    disabled = false;
    selectedItem: any;
    selectedItemNumber: string = '';
    labelMap: typeof LabelMap;
    constructor(
        private searchService: SearchService,
        private pmDataTransService: PartMasterDataTransService
    ) {
        this.labelMap = LabelMap;
        this.search();
    }
    ngAfterViewInit(): void {
        this.focusOnItemNumber();
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

        this.pmDataTransService.selectedCRUD$.subscribe(crud => {
            if (crud === CRUD.Add || crud === CRUD.Edit && this.componentType === ComponentType.PartMaster) {

                this.disabled = true;
            }
            else {
                this.disabled = false;
                if (crud === CRUD.Cancel) {
                    if (!!this.selectedItem) {
                        this.selectedItemNumber = this.selectedItem.itemNumber;
                        this.pmDataTransService.selectedItemChanged(this.selectedItem, this.componentType);
                    }
                }
                else if (crud === CRUD.DoneDelete) {
                    this.search();
                }
            }
        });

    }

    openMFRCodeBox() {
        this.mfrVar?.instance.open();
    }

    openItemClassCodeBox() {
        this.itemClassVar?.instance.open();
    }

    openItemCodeCodeBox() {
        this.itemCodeVar?.instance.open();
    }


    handleSubmit(e: any) {
        console.log(this.pmsearchInfo);
        this.search();
        e.preventDefault();
    }
    search() {
        this.searchService.getItemNumberSearchResults(this.pmsearchInfo).subscribe(r => {
            this.pmSearchResults = r;
            /* if (this.pmSearchResults.length !== 0) {
                this.pmDataTransService.selectedItemChanged(this.pmSearchResults[0], this.componentType);
                this.selectedItem = this.pmSearchResults[0];
                this.selectedItemNumber = this.selectedItem.itemNumber;
            } */
        });
    }
    onSelectionChanged(e: any) {
        console.log(e);
        if (!!e.addedItems[0]) {
            this.selectedItem = e.addedItems[0];
            console.log(this.selectedItem);
            this.selectedItemNumber = this.selectedItem.itemNumber;
            this.pmDataTransService.selectedItemChanged(this.selectedItem, this.componentType);
        }
    }

    private focusOnItemNumber() {
        setTimeout(() => {
            (<HTMLInputElement>document.getElementsByName('itemnumber')[0]).value =
                '';
            document.getElementsByName('itemnumber')[0].focus();
        }, 0);
    }
}