import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { DevExpressModule } from 'src/app/devexpress.module'; 
import { InventoryStatusComponent } from '../pages/components/InventoryStatus/inventory.status.component';
import { PMSearchComponent } from '../pages/components/PartMaster/search/pmsearch.component';
import { AddLabelDirective } from './directives/add.label.directive';
import { DisplayLabelDirective } from './directives/display.label.directive';
@NgModule({
    imports: [DevExpressModule,CommonModule],
    declarations: [ PMSearchComponent,DisplayLabelDirective,AddLabelDirective ,InventoryStatusComponent],
    exports: [  PMSearchComponent,DisplayLabelDirective,AddLabelDirective ,InventoryStatusComponent ]
})
export class SharedModule { }
