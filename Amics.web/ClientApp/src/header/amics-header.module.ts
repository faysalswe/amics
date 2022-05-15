import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
import { AmicsHeaderComponent } from './amics-header/amics-header.component';
import { AmicsBrandingComponent } from './amics-branding/amics-branding.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';

@NgModule({
  imports: [CommonModule, FormsModule, RouterModule],
  declarations: [
    AmicsHeaderComponent,
    AmicsBrandingComponent,
    NavMenuComponent
  ],
  exports: [
    AmicsHeaderComponent
  ]
})
export class AmicsHeaderModule {}
