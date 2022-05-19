import { NgModule } from "@angular/core";
import { DxButtonModule, DxContextMenuModule, DxDataGridModule, DxFormModule, DxListModule, DxSortableModule, DxTabPanelModule, DxTemplateModule } from "devextreme-angular";

@NgModule({
    imports: [ 
      DxButtonModule,
      DxSortableModule,
      DxTabPanelModule,
      DxListModule,
      DxTemplateModule,
      DxDataGridModule, 
      DxFormModule,
      DxContextMenuModule
    ], 
    exports: [
        DxButtonModule,
        DxSortableModule,
        DxTabPanelModule,
        DxListModule,
        DxTemplateModule,
        DxDataGridModule, 
         DxFormModule,
         DxContextMenuModule
    ]
  })
  export class DevExpressModule { }
  