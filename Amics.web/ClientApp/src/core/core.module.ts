import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, ErrorHandler, NgModule, Optional, SkipSelf } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
import { AmicsHeaderModule } from 'src/header/amics-header.module';
import { AccessDeniedComponent } from './components/acess-denied/access-denied.component';
import { AppFooterComponent } from './components/app-footer/app-footer.component';
import { ContactUSComponent } from './components/contact-us/contact-us.component';
import { AppErrorHandler } from './services/app-errror-handler';
import { HttpCacheControlService } from './services/http-cache-control.service';

@NgModule({
  imports:      [  CommonModule,HttpClientModule, RouterModule, FormsModule, ReactiveFormsModule, AmicsHeaderModule],
  providers:    [ {
    provide: ErrorHandler,
    useClass: AppErrorHandler,
  },{
    provide: HTTP_INTERCEPTORS,
    useClass: HttpCacheControlService,
    multi: true,
  } ],
  declarations: [ AppFooterComponent, AccessDeniedComponent, ContactUSComponent ],
  exports:      [ AppFooterComponent, AccessDeniedComponent, ContactUSComponent, AmicsHeaderModule ],
  bootstrap:    [  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class CoreModule { constructor(
    @Optional()
    @SkipSelf()
    core: CoreModule
  ) {
    if (core) {
      throw new Error("Core Module can only be imported in AppModule.");
    }
  } }