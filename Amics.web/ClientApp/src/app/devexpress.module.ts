import { NgModule } from "@angular/core";
import { DxButtonModule, DxDataGridModule, DxFormModule, DxListModule, DxSortableModule, DxTabPanelModule, DxTemplateModule } from "devextreme-angular";

@NgModule({
    imports: [ 
      DxButtonModule,
      DxSortableModule,
      DxTabPanelModule,
      DxListModule,
      DxTemplateModule,
      DxDataGridModule, 
      DxFormModule,
    ], 
    exports: [
        DxButtonModule,
        DxSortableModule,
        DxTabPanelModule,
        DxListModule,
        DxTemplateModule,
        DxDataGridModule, 
         DxFormModule,
    ]
  })
  export class DevExpressModule { }
  