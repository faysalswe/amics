import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";  
import { DragDirective } from "./directives/drag-drop.directive";
import { MaterialModule } from "./material.module";  
import { DeleteFileDialogComponent } from "./components/delete-file-dialog/delete-file-dialog.component";
import { OverlayModule } from "@angular/cdk/overlay";
import { PortalModule } from "@angular/cdk/portal";
import { MaterialFileUploadComponent } from "./components/material-file-upload/material-file-upload.component";
@NgModule({
  imports: [
    MaterialModule,
    CommonModule, 
    OverlayModule,
    PortalModule,
  ],
  declarations: [
    MaterialFileUploadComponent,
    DragDirective, 
    DeleteFileDialogComponent,
  ],
  entryComponents: [DeleteFileDialogComponent],
  exports: [
    MaterialFileUploadComponent,
    MaterialModule, 
    CommonModule, 
    DragDirective, 
    DeleteFileDialogComponent,
    OverlayModule,
    PortalModule,
  ],
})
export class SharedModule {}
