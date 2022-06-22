import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DevExpressModule } from 'src/app/devexpress.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { PMDetailsComponent } from './details/pmdetails.component';
import { PMFooterComponent } from './footer/pmfooter.component';
import { PartMasterComponent } from './partmaster.component';
@NgModule({
    imports: [DevExpressModule,CommonModule,SharedModule],
    declarations: [PartMasterComponent, PMDetailsComponent, PMFooterComponent],
    exports: [PartMasterComponent, PMDetailsComponent, PMFooterComponent]
})
export class PartMasterModule { }
