import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/components/home/home.component';
import { ProfileComponent } from './pages/components/profile/profile.component';
import { TasksComponent } from './pages/components/tasks/tasks.component'; 
import { IncreaseInventoryComponent } from "./pages/components/IncreaseInventory/increase.inventory.component";
import { InquiryComponent } from './pages/components/inquiry/inquiry.component';
import { EquipmentComponent } from './pages/components/equipment/equipment.component';

const routes: Routes = [
  {
    path: 'tasks',
    component: TasksComponent,
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
