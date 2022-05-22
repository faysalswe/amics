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

export function appUserServiceFactory(authService: AuthService): Function {
  return () => authService.getUser();
}


export function appEnvironmentFactory(
  environmentService: AppSettingsService
): Function {
  return () => environmentService.getAppSettings();
}

@NgModule({
  declarations: [
    AppComponent
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
    HttpClientModule
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
