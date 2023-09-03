import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NewsListComponent } from './components/news/news-list/news-list.component';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ClassifyNewsModalComponent } from './components/news/classify-news-modal/classify-news-modal.component';
import { CreateSubscriberModalComponent } from './components/subscribe/create-subscriber-modal/create-subscriber-modal.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NewsCountsComponent } from './components/news-counts/news-counts/news-counts.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatIconModule} from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from  '@angular/material/input';
import {MatSelectModule} from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule } from '@angular/material/core';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import { DatePipe } from '@angular/common';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { PublisherApprovalComponent } from './components/publisher-approval/publisher-approval/publisher-approval.component';
import { ReviewRequestedNewsComponent } from './components/review-requests/review-requested-news/review-requested-news.component';
import { ModeratorSignupComponent } from './components/moderator-signup/moderator-signup/moderator-signup.component';
import { InviteModeratorModalComponent } from './components/invite-moderator/invite-moderator-modal/invite-moderator-modal.component';
import { ReviewRequestNewsModalComponent } from './components/news/ReviewRequestNewsModal/review-request-news-modal/review-request-news-modal.component';
import { SubmitReviewModalComponent } from './components/review-requests/submit-review-modal/submit-review-modal.component';
import { LayoutComponent } from './components/layout/layout.component';
import { MatTabsModule } from '@angular/material/tabs';
import { ModeratorInvitationComponent } from './components/invite-moderator/moderator-invitation/moderator-invitation.component';
import { TabbedViewComponent } from './components/tabbed-view/tabbed-view/tabbed-view.component';

@NgModule({
  declarations: [
    AppComponent,
    NewsListComponent,
    ClassifyNewsModalComponent,
    CreateSubscriberModalComponent,
    NewsCountsComponent,
    LoginComponent,
    SignupComponent,
    PublisherApprovalComponent,
    ReviewRequestedNewsComponent,
    ModeratorSignupComponent,
    InviteModeratorModalComponent,
    ReviewRequestNewsModalComponent,
    SubmitReviewModalComponent,
    LayoutComponent,
    ModeratorInvitationComponent,
    TabbedViewComponent,
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    FormsModule,
    BrowserAnimationsModule,
    MatDatepickerModule,
    MatInputModule,
    MatButtonModule, 
    MatMenuModule, 
    MatIconModule,
    MatSelectModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatButtonToggleModule,
    CanvasJSAngularChartsModule,
    ReactiveFormsModule,
    MatTabsModule
  ],
  entryComponents: [ClassifyNewsModalComponent],
  providers: [
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
