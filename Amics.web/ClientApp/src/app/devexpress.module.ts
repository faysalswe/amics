import { NgModule } from "@angular/core";
import { DxButtonModule, DxCheckBoxModule, DxContextMenuModule, DxDataGridModule, DxFormModule, DxListModule, DxNumberBoxModule, DxSelectBoxModule, DxSortableModule, DxTabPanelModule, DxTemplateModule, DxBoxModule, DxRadioGroupModule, DxTextAreaModule, DxResponsiveBoxModule } from "devextreme-angular";
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
    DxTextAreaModule,
    DxResponsiveBoxModule
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
    DxTextAreaModule,
    DxResponsiveBoxModule
  ]
})
export class DevExpressModule { }
