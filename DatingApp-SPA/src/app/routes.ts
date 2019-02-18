import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';


export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        // System is based on the first match, so when we hit this empty path we recursively go
        //into childrens and try to find match. So that would be "" + "{childrens path}"
        path: '',
        runGuardsAndResolvers: "always",
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MemberListComponent}, // "" + "members" = "members"
            { path: 'messages', component: MessagesComponent},
            { path: 'lists', component: ListsComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'},

];


   // Second variant is to add "canActivate: [AuthGuard]}" to every path after comma.Like so :
  //  { path: 'members', component: MemberListComponent}, canActivate: [AuthGuard]},

