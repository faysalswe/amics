import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component'; 
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component'; 
import { CoreModule } from 'src/core/core.module';
import { UserService } from 'src/core/services/user.service';
import { AppRoutingModule } from './app.routing';
import { LoginComponent } from './login/login.component';
import { SharedModule } from './shared/shared.module'; 
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { FlexLayoutModule } from '@angular/flex-layout';
 

@NgModule({
  declarations: [
    AppComponent, 
    HomeComponent,
    LoginComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CoreModule,
    AppRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    BrowserAnimationsModule,
    FlexLayoutModule
    ],
  providers: [ ],
  bootstrap: [AppComponent]
})
export class AppModule { }
