import {Routes} from '@angular/router';
import { HomeComponent } from './app/home/home.component';
import { MemberListComponent } from './app/members/member-list/member-list.component';
import { MessagesComponent } from './app/messages/messages.component';
import { ListsComponent } from './app/lists/lists.component';
import { AuthGuard } from './app/_guards/auth.guard';
import { MemberDetailComponent } from './app/members/member-detail/member-detail.component';
import { MemberDetailResolver } from './app/_resolvers/member-detail.resolver';
import { MemberListResolver } from './app/_resolvers/member-list.resolver';
import { MemberEditComponent } from './app/members/member-edit/member-edit.component';
import { MemberEditResolver } from './app/_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './app/_guards/prevent-unsaved-guard';

// this one is for restricting router with AuthGurad
// export const appRoutes: Routes = [
//     { path: 'home', component: HomeComponent },
//     { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] },
//     { path: 'messages', component: MessagesComponent },
//     { path: 'lists', component: ListsComponent },
//     { path: '**', redirectTo: 'home', pathMatch: 'full' }
// ];

// this is for adding authguard to all routes at once using childern
export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers : 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent,
              resolve: {dbusers: MemberListResolver}},

            { path: 'members/:id', component: MemberDetailComponent,
              resolve: {dbuser: MemberDetailResolver}}, // use this dbuser(same name) in component

            {path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
            canDeactivate: [PreventUnsavedChanges]},
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
