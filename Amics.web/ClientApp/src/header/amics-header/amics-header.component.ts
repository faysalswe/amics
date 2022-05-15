import { Component, Input } from '@angular/core';
import { InternalRoute } from "src/app/shared/models/internal-route";

@Component({
  selector: 'amics-header',
  templateUrl: './amics-header.component.html',
  styleUrls: ['./amics-header.component.scss']
})
export class AmicsHeaderComponent { 
  @Input() user: string; 
  @Input() internalRoutes: InternalRoute[]; 
  @Input() showTestWarning?: boolean = false;

  constructor() { }
 }
