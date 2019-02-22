import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';


export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        // System is based on the first match, so when we hit this empty path we recursively go
        // into childrens and try to find match. So that would be "" + "{childrens path}"
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
                                                                // passing data to member list component
            { path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}}, // "" + "members" = "members"
            { path: 'members/:id', component: MemberDetailComponent,
            // adding route resolvers to get data before activating route and then subscribing to fetch data
            resolve: {user: MemberDetailResolver}},
            { path: 'messages', component: MessagesComponent},
            { path: 'lists', component: ListsComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'},

];


   // Second variant is to add "canActivate: [AuthGuard]}" to every path after comma.Like so :
  //  { path: 'members', component: MemberListComponent}, canActivate: [AuthGuard]},

