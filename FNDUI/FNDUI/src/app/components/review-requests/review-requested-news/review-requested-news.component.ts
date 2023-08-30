import { Component } from '@angular/core';
import { RequestReviewModel } from 'src/app/models/request-review.model';
import { ReviewRequestedNewsListModel } from 'src/app/models/review-requested-newsList.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ModeratorService } from 'src/app/services/moderator/moderator.service';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { ReviewRequestNewsModalComponent } from '../../news/ReviewRequestNewsModal/review-request-news-modal/review-request-news-modal.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SubmitReviewModalComponent } from '../submit-review-modal/submit-review-modal.component';
import { NewsService } from 'src/app/services/news.service';

@Component({
  selector: 'app-review-requested-news',
  templateUrl: './review-requested-news.component.html',
  styleUrls: ['./review-requested-news.component.css']
})
export class ReviewRequestedNewsComponent {
  public role:string = "";
  public userId:number=0;
  requestedReview:ReviewRequestedNewsListModel[] | undefined;
  requestedReviewFeedback:ReviewRequestedNewsListModel[] | undefined;
  IsPending: boolean = false;
  isApproved = false;
  isRejected = false;
  reviewRequestedNewsId:number=0;

  constructor(private auth:AuthService, private userStore:UserStoreService, private moderatorService:ModeratorService,private newsService:NewsService, private modalService:NgbModal){}

  ngOnInit(){
    this.userStore.getRoleFromStore()
    .subscribe(val=>{
      const roleFromToken = this.auth.getRoleFromToken();
      const userId = this.auth.getprimarySidFromToken();
      this.role = val || roleFromToken;
      this.userId=parseInt(userId);
    })
    if(this.role=='Moderator')
        this.GetAllReviewRequestedNews();
    if(this.role=='Publisher')
        this.GetReviewRequestedNewsByPublisherId(this.userId);  
  }

  GetAllReviewRequestedNews(){
    this.moderatorService.GetAllReviewRequestedNews()
    .subscribe({
      next:(requestedReview) => {
        this.requestedReview = requestedReview;
        //console.log(requestedReview);
      },
      error:(response: any) =>{
        console.log(response);  
      }
    })
  }

  openSubmitReviewModal(newId:number){
    this.reviewRequestedNewsId = newId;
    const modalRef = this.modalService.open(SubmitReviewModalComponent);
    modalRef.componentInstance.newId = newId;
  }

  GetReviewRequestedNewsByPublisherId(publisherId:number){
    this.newsService.GetReviewRequestedNewsByPublisherId(this.userId)
    .subscribe({
      next:(requestedReviewFeedback) => {
        this.requestedReviewFeedback = requestedReviewFeedback;
        console.log(requestedReviewFeedback);
      },
      error:(response: any) =>{
        console.log(response);  
      }
    })
  }

}

