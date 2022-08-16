import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { exportDataGrid } from "devextreme/excel_exporter";
import { Workbook } from "exceljs";
import * as saveAs from "file-saver";
import { PopUpAction } from "src/app/pages/models/pmChildType";
import { pmWHLocation } from "src/app/pages/models/pmWHLocation";
import { PartMasterService } from "src/app/pages/services/partmaster.service";
import { PartMasterDataTransService } from "src/app/pages/services/pmdatatransfer.service";

@Component({
    selector: "app-pmViewLocation",
    templateUrl: "./ViewLocation.component.html",
    styleUrls: ['./viewLocation.component.scss']
})
export class ViewLocationComponent implements OnInit {

    @Output() viewLocationPopup: EventEmitter<any> = new EventEmitter<any>();
    @Input() Visibility:boolean = false;
    @Input() PMDetailId: any;

    secUserId = 'E02310D5-227F-4DB8-8B42-C6AE3A3CB60B';
    pmWHLocations: pmWHLocation[] = [];

    constructor(
        private pmService: PartMasterService
    ) {
    }
    
    ngOnInit(): void {
        this.getLocations();
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
                saveAs(
                    new Blob([buffer], { type: 'application/octet-stream' }),
                    'DataGrid.xlsx'
                );
            });
        });
        e.cancel = true;
    }

    getLocations(wh: string = '') {
        this.pmService
            .getViewWHLocation(this.PMDetailId, this.secUserId, wh)
            .subscribe((x) => (this.pmWHLocations = x));
    }

    HidingVL(){
        this.viewLocationPopup.emit();
    }
}