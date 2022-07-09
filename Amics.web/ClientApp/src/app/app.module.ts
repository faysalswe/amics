import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { SideNavOuterToolbarModule, SideNavInnerToolbarModule, SingleCardModule } from './layouts';
import { FooterModule, ResetPasswordFormModule, CreateAccountFormModule, ChangePasswordFormModule, LoginFormModule } from './shared/components';
import { AuthService, ScreenService, AppInfoService } from './shared/services';
import { UnauthenticatedContentModule } from './unauthenticated-content';
import { AppRoutingModule } from './app-routing.module';
import { DevExpressModule } from './devexpress.module';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpCacheControlService } from './shared/services/http-cache-control.service';
import { DomainReplaceIterceptor } from './shared/services/DomainReplaceInterceptor';
import { AppSettingsService } from './shared/services/app-settings.service';
import { PartMasterModule } from './pages/components/PartMaster/partmaster.module';
import { HomeComponent } from './pages/components/home/home.component';
import { ProfileComponent } from './pages/components/profile/profile.component';
import { TasksComponent } from './pages/components/tasks/tasks.component';
import { HostComponent } from './pages/components/host/host.component';
import { ResponsiveComponent } from './pages/components/PartMaster/responsive/responsive.component';
import { IncreaseInventoryComponent } from './pages/components/IncreaseInventory/increase.inventory.component';
import { InquiryComponent } from './pages/components/inquiry/inquiry.component';
import { InventoryStatusComponent } from './pages/components/InventoryStatus/inventory.status.component';
import { AppInitialDataService } from './shared/services/app.initial.data.service';
import { SerialDocumentsComponent } from './pages/components/serial-documents/serial-documents.component';
import { ChangeSerialComponent } from './pages/components/change-serial/change-serial.component';
import { ReportsComponent } from './pages/components/reports/reports.component';
import { MdatComponent } from './pages/components/mdat/mdat.component';
import { SharedModule } from './shared/shared.module';
import { EquipmentComponent } from './pages/components/equipment/equipment.component';
import { ShipmentComponent } from './pages/components/shipment/shipment.component';
import { Report2Component } from './pages/components/report2/report2.component';
import { BulkTransferComponent } from './pages/components/bulk-transfer/bulk-transfer.component';
import {ReactiveFormsModule} from "@angular/forms";
import { TransLogComponent } from './pages/components/IncreaseInventory/trans-log/trans-log.component';
import { TransLogSubDetailsComponent } from './pages/components/IncreaseInventory/trans-log/trans-log-sub-details/trans-log-sub-details.component';
import { StatusComponent } from './shared/components/status/status.component';
import { DecreaseInventoryComponent } from './pages/components/DecreaseInventory/decrease.inventory.component';

export function appUserServiceFactory(authService: AuthService): Function {
  return () => authService.getUser();
}
export function appInitialDataServiceFactory(dataLoaderService: AppInitialDataService): Function {
  return () => dataLoaderService.loadData();
}

export function appEnvironmentFactory(
  environmentService: AppSettingsService
): Function {
  return () => environmentService.getAppSettings();
}

@NgModule({
  declarations: [
    AppComponent, HomeComponent,
    ProfileComponent,
    TasksComponent,
    HostComponent,
    ResponsiveComponent,
    IncreaseInventoryComponent,
    InquiryComponent,
    SerialDocumentsComponent,
    ChangeSerialComponent,
    ReportsComponent,
    MdatComponent,
    EquipmentComponent,
    TransLogComponent,
    TransLogSubDetailsComponent,
    ShipmentComponent,
    Report2Component,
    BulkTransferComponent,
    StatusComponent,
    DecreaseInventoryComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    SideNavOuterToolbarModule,
    SideNavInnerToolbarModule,
    SingleCardModule,
    FooterModule,
    ResetPasswordFormModule,
    CreateAccountFormModule,
    ChangePasswordFormModule,
    LoginFormModule,
    UnauthenticatedContentModule,
    AppRoutingModule,
    DevExpressModule,
    HttpClientModule,
    PartMasterModule,
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appEnvironmentFactory,
      deps: [AppSettingsService],
      multi: true,
    },
    {
      provide: APP_INITIALIZER,
      useFactory: appUserServiceFactory,
      deps: [AuthService],
      multi: true,
    }, {
      provide: APP_INITIALIZER,
      useFactory: appInitialDataServiceFactory,
      deps: [AppInitialDataService],
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: DomainReplaceIterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpCacheControlService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
