import { Component, Input, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RequestReviewModel } from 'src/app/models/request-review.model';
import { NewsService } from 'src/app/services/news.service';

@Component({
  selector: 'app-review-request-news-modal',
  templateUrl: './review-request-news-modal.component.html',
  styleUrls: ['./review-request-news-modal.component.css']
})
export class ReviewRequestNewsModalComponent {
  @Input() newId: number = 0;
  constructor(public activeModal: NgbActiveModal, private newsService: NewsService) { }

  //@ViewChild('newsId') newsId: any;
  
  @ViewChild('contentInput') contentInput: any;
  result: any= '';
  error:any='';
  showForm: boolean = true;

  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    // Handle the form submission here
    // You can access the form data and perform any necessary operations
    //const newsTitle = this.titleInput.nativeElement.value;
    const newsContent = this.contentInput.nativeElement.value;
    //this.activeModal.close();

    // Create an object with the data to be sent
    const requestReviewModel: RequestReviewModel = {
      newsId: this.newId,
      comment: newsContent
    };

    // Make a POST request to the API endpoint
    this.newsService.requestReview(requestReviewModel).subscribe(
      response => {
        // Handle the response from the API
        console.log('API response:', response);
            this.result = response.status;
            this.showForm = false;
        

        // Reset the form or perform further operations
      },
      error => {
        // Handle any errors that occur during the request
        this.error = error;
          // Handle any errors that occur during the request
          console.error('API error:', error);
      });
  }
}
