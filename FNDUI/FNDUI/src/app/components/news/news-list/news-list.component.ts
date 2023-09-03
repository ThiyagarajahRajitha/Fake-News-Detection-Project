import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { News } from 'src/app/models/news.model';
import { NewsService } from 'src/app/services/news.service';
import { ClassifyNewsModalComponent } from '../classify-news-modal/classify-news-modal.component';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ReviewRequestNewsModalComponent } from '../ReviewRequestNewsModal/review-request-news-modal/review-request-news-modal.component';
import { CreateSubscriberModalComponent } from '../../subscribe/create-subscriber-modal/create-subscriber-modal.component';
import { SubmitReviewModalComponent } from '../../review-requests/submit-review-modal/submit-review-modal.component';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-news-list',
  templateUrl: './news-list.component.html',
  styleUrls: ['./news-list.component.css']
})
export class NewsListComponent implements OnInit{
  
   news: News[] = [];
   checkboxValue: boolean = false;
   username:string='';
   userRole:String='';
   userId:String='';
   uid:number=0;
   reviewRequestedNewsId : number=0;
   max: number = 500;
   publisherFilterControl = new FormControl('fakeOnly');
   publicFilterControl = new FormControl('all');
   ModeratorFilterControl = new FormControl('pendingOnly');

  constructor(private newsService: NewsService, private modalService: NgbModal, private userStore:UserStoreService, private auth:AuthService){}

  openSubscribeModal(){
    const modalRef = this.modalService.open(CreateSubscriberModalComponent);
  }
  
  openClassifyNewsModal() {
    const modalRef = this.modalService.open(ClassifyNewsModalComponent);
  }

  openReviewRequestNewsModal(newId:number){
    this.reviewRequestedNewsId = newId;
    const modalRef = this.modalService.open(ReviewRequestNewsModalComponent);
    modalRef.componentInstance.newId = newId;
  }

  openSubmitReviewModal(newId:number){
    this.reviewRequestedNewsId = newId;
    const modalRef = this.modalService.open(SubmitReviewModalComponent);
    modalRef.componentInstance.newId = newId;
  }

  ngOnInit():void{
    this.userRole =this.auth.getRoleFromToken();
    this.userId = this.auth.getprimarySidFromToken();
    this.username = this.auth.getNameFromToken();
    this.uid = Number(this.userId);
    
    if(this.userRole=='Publisher') {
      this.getNewsByPublisher("fakeOnly");
      this.publisherFilterControl.valueChanges.subscribe( filter => {
        if (filter) {
          this.getNewsByPublisher(filter)
        }
      });
    } else if (this.userRole=='Moderator') {
      this.GetAllReviewRequestedNews("pendingOnly");
      this.ModeratorFilterControl.valueChanges.subscribe( filter => {
        if (filter) {
          this.GetAllReviewRequestedNews(filter)
        } else {
          this.GetAllReviewRequestedNews("pendingOnly")
        }
      });
    }
    else {
      this.getAllNews(false);
      this.publicFilterControl.valueChanges.subscribe( filter => {
        if (filter == "fakeOnly") {
          this.getAllNews(true)
        } else {
          this.getAllNews(false)
        }
      });
    }
  }

  getAllNews(isFakeOnly:boolean){
    this.newsService.getAllNews(isFakeOnly)
    .subscribe({
      next:(news) => {
        this.news = news;
        //console.log(news);
      },
      error:(response) =>{
        console.log(response);
        
      }
    })
  }

  getNewsByPublisher(filter:string){
    this.newsService.getNewsByPublisherId(this.uid, filter)
    .subscribe({
      next:(news) => {
        this.news = news;
        //console.log(news);
      },
      error:(response) =>{
        console.log(response);
        
      }
    })
  }

  GetAllReviewRequestedNews(filter:string){
    this.newsService.GetAllReviewRequestedNews(filter)
    .subscribe({
      next:(news) => {
        this.news = news;
        //console.log(news);
      },
      error:(response: any) =>{
        console.log(response);  
      }
    })
  }
}
