import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { News } from 'src/app/models/news.model';
import { NewsService } from 'src/app/services/news.service';
import { ClassifyNewsModalComponent } from '../classify-news-modal/classify-news-modal.component';
import { UserStoreService } from 'src/app/services/user-store/user-store.service';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ReviewRequestNewsModalComponent } from '../ReviewRequestNewsModal/review-request-news-modal/review-request-news-modal.component';
import { CreateSubscriberModalComponent } from '../../subscribe/create-subscriber-modal/create-subscriber-modal.component';

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
    //{
  //   id:54,
  //   url:'dsgsdsg.com',
  //   title:'Covid Today',
  //   description:'dfbdsf egjhsd ewotqr qrwt',
  //   publisher_id:234,
  //   classification_Decision:true
  // }

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

  ngOnInit():void{
    this.userRole =this.auth.getRoleFromToken();
    this.userId = this.auth.getprimarySidFromToken();
    this.username = this.auth.getNameFromToken();
    this.uid = Number(this.userId);
    if(this.userRole=='Publisher')
      this.getNewsByPublisher();
    else
      this.getAllNews();
  }

  getAllNews(){
    this.newsService.getAllNews(this.checkboxValue)
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

  onCheckboxChange() {
    this.getAllNews();
  }

  getNewsByPublisher(){
    this.newsService.getNewsByPublisherId(this.uid, this.checkboxValue)
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
}
