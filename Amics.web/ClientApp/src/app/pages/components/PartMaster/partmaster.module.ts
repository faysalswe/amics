import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DevExpressModule } from 'src/app/devexpress.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { BomComponent } from './bom/bom.component';
import { PMDetailsComponent } from './details/pmdetails.component';
import { PMFooterComponent } from './footer/pmfooter.component';
import { NotesComponent } from './notes/notes.component';
import { PartMasterComponent } from './partmaster.component';
import { PoComponent } from './po/po.component';
import { ViewLocationComponent } from './viewLocation/viewLocation.component';
import { ViewSerialComponent } from './viewSerial/viewSerial.component';
@NgModule({
    imports: [DevExpressModule,CommonModule,SharedModule],
    declarations: [PartMasterComponent, PMDetailsComponent, PMFooterComponent, ViewLocationComponent,
        ViewSerialComponent, BomComponent, PoComponent, NotesComponent],
    exports: [PartMasterComponent, PMDetailsComponent, PMFooterComponent, ViewLocationComponent,
        ViewSerialComponent, BomComponent, PoComponent, NotesComponent]
})
export class PartMasterModule { }
