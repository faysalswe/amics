import { NgModule } from "@angular/core";
import { DxButtonModule, DxCheckBoxModule, DxContextMenuModule, DxDataGridModule, DxFormModule, DxListModule, DxNumberBoxModule, DxSelectBoxModule, DxSortableModule, DxTabPanelModule, DxTemplateModule } from "devextreme-angular";

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
      DxNumberBoxModule
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
         DxNumberBoxModule
    ]
  })
  export class DevExpressModule { }
  