import { Component, NgModule, Input } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { DevExpressModule } from 'src/app/devexpress.module';

@Component({
  selector: 'app-user-panel',
  templateUrl: 'user-panel.component.html',
  styleUrls: ['./user-panel.component.scss']
})

export class UserPanelComponent {
  @Input()
  menuItems: any;

  @Input()
  menuMode!: string;

  @Input()
  user?: string ;

  constructor() {}
}

@NgModule({
  imports: [ 
    CommonModule,
    DevExpressModule
  ],
  declarations: [ UserPanelComponent ],
  exports: [ UserPanelComponent ]
})
export class UserPanelModule { }
