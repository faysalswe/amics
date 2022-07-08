import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/components/home/home.component';
import { ProfileComponent } from './pages/components/profile/profile.component'; 
import { IncreaseInventoryComponent } from "./pages/components/IncreaseInventory/increase.inventory.component";
import { InquiryComponent } from './pages/components/inquiry/inquiry.component';
import { EquipmentComponent } from './pages/components/equipment/equipment.component';
import { ShipmentComponent } from './pages/components/shipment/shipment.component';
import { Report2Component } from './pages/components/report2/report2.component';
import { BulkTransferComponent } from './pages/components/bulk-transfer/bulk-transfer.component';
import { ChangeLocationComponent } from './pages/components/change-location/change-location.component';

const routes: Routes = [
  {
    path: 'tasks',
    component: ChangeLocationComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'login-form',
    component: LoginFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'reset-password',
    component: ResetPasswordFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'create-account',
    component: CreateAccountFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'change-password/:recoveryCode',
    component: ChangePasswordFormComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'increase-inventory',
    component: IncreaseInventoryComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'inquiry',
    component: InquiryComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'equipment',
    component: EquipmentComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'shipment',
    component: ShipmentComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: 'report2',
    component: Report2Component,
    canActivate: [AuthGuardService]
  },
  {
    path: 'bulkTransfer',
    component: BulkTransferComponent,
    canActivate: [AuthGuardService]
  },
  {
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [

  ]
})
export class AppRoutingModule { }
