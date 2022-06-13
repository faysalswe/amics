import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DevExpressModule } from 'src/app/devexpress.module';
import { PMDetailsComponent } from './details/pmdetails.component';
import { PMFooterComponent } from './footer/pmfooter.component';
import { PartMasterComponent } from './partmaster.component';
import { PMSearchComponent } from './search/pmsearch.component';
@NgModule({
    imports: [DevExpressModule,CommonModule],
    declarations: [PartMasterComponent, PMSearchComponent, PMDetailsComponent, PMFooterComponent],
    exports: [PartMasterComponent, PMSearchComponent, PMDetailsComponent, PMFooterComponent]
})
export class PartMasterModule { }
