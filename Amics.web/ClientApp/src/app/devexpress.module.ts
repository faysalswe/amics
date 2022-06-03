import { NgModule } from "@angular/core";
import { DxButtonModule, DxCheckBoxModule, DxContextMenuModule, DxDataGridModule, DxFormModule, DxListModule, DxNumberBoxModule, DxSelectBoxModule, DxSortableModule, DxTabPanelModule, DxTemplateModule, DxBoxModule, DxRadioGroupModule, DxTextAreaModule, DxResponsiveBoxModule, DxAutocompleteModule, DxLookupModule, DxButtonGroupModule } from "devextreme-angular";
import dxButtonGroup from "devextreme/ui/button_group";
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
    DxResponsiveBoxModule,
    DxAutocompleteModule,
    DxLookupModule,
    DxButtonGroupModule
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
    DxResponsiveBoxModule,
    DxAutocompleteModule,
    DxLookupModule,
    DxButtonGroupModule
  ]
})
export class DevExpressModule { }
