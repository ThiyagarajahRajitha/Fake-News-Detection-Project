import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewsListComponent } from './components/news/news-list/news-list.component';
import { NewsCountsComponent } from './components/news-counts/news-counts/news-counts.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './guards/auth.guard';
import { PublisherApprovalComponent } from './components/publisher-approval/publisher-approval/publisher-approval.component';
import { ReviewRequestedNewsComponent } from './components/review-requests/review-requested-news/review-requested-news.component';
import { ModeratorSignupComponent } from './components/moderator-signup/moderator-signup/moderator-signup.component';
import { InviteModeratorModalComponent } from './components/invite-moderator/invite-moderator-modal/invite-moderator-modal.component';
import { LayoutComponent } from './components/layout/layout.component';
import { ModeratorInvitationComponent } from './components/invite-moderator/moderator-invitation/moderator-invitation.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: 'news-list', pathMatch: 'full' }, // Redirect to 'news-list'
      { path: 'news-list', component: NewsListComponent }, // Default route
      // {
      //   path:'news',
      //   component:NewsListComponent,
      // },
      {
        path:'publisher-approval',
      component: PublisherApprovalComponent,
      canActivate:[AuthGuard]
      },
      {
        path: 'moderator-invitation',
        component:ModeratorInvitationComponent,
        canActivate:[AuthGuard],
      },
      {
        path: 'review-requests',
        component: ReviewRequestedNewsComponent,
        canActivate:[AuthGuard]
      },
      {
        path:'dashboard',
        component: NewsCountsComponent,
        canActivate:[AuthGuard]
      }
      // { path: '', component: NewsListComponent },
      // { path: '', redirectTo: 'publisher-approval', pathMatch: 'full' },
      
      // { path: 'about', component: AboutComponent },
      // { path: 'services', component: ServicesComponent },
      // { path: 'contact', component: ContactComponent },
    ]
  },
//   {
//   path:'',
//   component:NewsListComponent
// },



{
  path: 'login',
  component: LoginComponent
},
{
  path: 'signup',
  component: SignupComponent
},
// {
//   path:'publisher-approval',
//   component: PublisherApprovalComponent,
//   canActivate:[AuthGuard]
// },

{
  path: 'moderator-signup',
  component:ModeratorSignupComponent
},
{
   path: 'invite-moderator',
   component:InviteModeratorModalComponent
 }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
