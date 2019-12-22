import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
// https://github.com/auth0/angular2-jwt
import { JwtModule } from '@auth0/angular-jwt'; // this is for JWT token headers to be sent in each request
import { NgxGalleryModule } from 'ngx-gallery';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptor } from './_services/error.interceptor';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from '../routes';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-guard';


// this is to get value from token
export function tokenGetter() {
   return localStorage.getItem('token');
 }
 // this is for issue faced my image gallary v5.10
export class CustomHammerConfig extends HammerGestureConfig  {
   overrides = {
       pinch: { enable: false },
       rotate: { enable: false }
   };
}
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      FormsModule,
      HttpClientModule,
      JwtModule.forRoot({
         config: {
           // tslint:disable-next-line: object-literal-shorthand
           tokenGetter: tokenGetter,
           whitelistedDomains: ['localhost:5001'],
           blacklistedRoutes: ['localhost:5001/api/auth']
         }
       }),
       BsDropdownModule.forRoot(),
       TabsModule.forRoot(),
       NgxGalleryModule,
       RouterModule.forRoot(appRoutes)
   ],
   providers: [
      AuthService,
      ErrorInterceptor,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PreventUnsavedChanges,
      { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig } // this is for issue faced my image gallary v5.10
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
