import { Component, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import ValidateForm from 'src/app/helpers/validateform';
import { RequestReviewModel } from 'src/app/models/request-review.model';
import { NewsService } from 'src/app/services/news.service';

@Component({
  selector: 'app-review-request-news-modal',
  templateUrl: './review-request-news-modal.component.html',
  styleUrls: ['./review-request-news-modal.component.css']
})
export class ReviewRequestNewsModalComponent {
  @Input() newId: number = 0;
  constructor(private fb:FormBuilder, public activeModal: NgbActiveModal, private newsService: NewsService) { }

  form = this.fb.group({
    newscontent: ['', Validators.required]
  }) 
  result: boolean= false;
  error:any='';
  showForm: boolean = true;

  dismissModal() {
    this.activeModal.dismiss();
  }

  onSubmit() {
    if(!this.form.valid) {
      ValidateForm.validateAllFormFields(this.form);
      return;
    }
    
    // Handle the form submission here
    // You can access the form data and perform any necessary operations
    //const newsTitle = this.titleInput.nativeElement.value;
    const newsContent = this.form.value.newscontent || '';
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
            this.result = true;
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
