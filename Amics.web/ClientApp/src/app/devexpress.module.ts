import { NgModule } from "@angular/core";
import { DxButtonModule, DxCheckBoxModule, DxContextMenuModule, DxDataGridModule, DxFormModule, DxListModule, DxNumberBoxModule, DxSelectBoxModule, DxSortableModule, DxTabPanelModule, DxTemplateModule, DxBoxModule, DxRadioGroupModule, DxTextAreaModule } from "devextreme-angular";
@NgModule({
  imports: [
    DxButtonModule,
    DxSortableModule,
    DxTabPanelModule,
    DxListModule,
    DxTemplateModule,
    DxDataGridModule,
    DxFormModule,
    DxContextMenuModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    DxNumberBoxModule,
    DxBoxModule,
    DxRadioGroupModule,
    DxTextAreaModule
  ],
  exports: [
    DxButtonModule,
    DxSortableModule,
    DxTabPanelModule,
    DxListModule,
    DxTemplateModule,
    DxDataGridModule,
    DxFormModule,
    DxContextMenuModule,
    DxCheckBoxModule,
    DxSelectBoxModule,
    DxNumberBoxModule,
    DxBoxModule,
    DxRadioGroupModule,
    DxTextAreaModule
  ]
})
export class DevExpressModule { }
