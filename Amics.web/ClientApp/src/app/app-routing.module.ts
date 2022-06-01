import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginFormComponent, ResetPasswordFormComponent, CreateAccountFormComponent, ChangePasswordFormComponent } from './shared/components';
import { AuthGuardService } from './shared/services';
import { HomeComponent } from './pages/components/home/home.component';
import { ProfileComponent } from './pages/components/profile/profile.component';
import { TasksComponent } from './pages/components/tasks/tasks.component';
import { DxDataGridModule, DxFormModule } from 'devextreme-angular';
import { DevExpressModule } from './devexpress.module';
import { CommonModule } from '@angular/common';
import { PartMasterComponent } from './pages/components/PartMaster/partmaster.component';
import { HostComponent } from './pages/components/host/host.component';
import { PMSearchComponent } from './pages/components/PartMaster/search/pmsearch.component';
import { PMDetailsComponent } from './pages/components/PartMaster/details/pmdetails.component';
import { ResponsiveComponent } from './pages/components/PartMaster/responsive/responsive.component';

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
    path: '**',
    redirectTo: 'home'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true }), DevExpressModule, CommonModule],
  providers: [AuthGuardService],
  exports: [RouterModule],
  declarations: [
    HomeComponent,
    ProfileComponent,
    TasksComponent,
    PartMasterComponent,
    PMSearchComponent,
    PMDetailsComponent,
    HostComponent,
    ResponsiveComponent
  ]
})
export class AppRoutingModule { }
